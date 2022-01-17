using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Dto.Auth.Request;
using Core.Dto.Auth.Response;
using Core.Entity;
using Core.Entity.Auth;
using Core.Exceptions;
using Core.Helpers;
using Core.Interfaces.Common;
using Core.Interfaces.Repositories;
using Core.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;

namespace Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMailService _mailService;
        private readonly JwtSettings _jwtSettings;
        private readonly IBaseRepository<ResetPasswordTokenHistory> _resetPasswordRepository;
        private readonly ICurrentUserService _currentUserService;

        public AuthService(UserManager<ApplicationUser> userManager,
            JwtSettings jwtSettings,
            SignInManager<ApplicationUser> signInManager,
            IMailService mailService, 
            IBaseRepository<ResetPasswordTokenHistory> resetPasswordRepository,
            ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _signInManager = signInManager;
            _mailService = mailService;
            _resetPasswordRepository = resetPasswordRepository;
            _currentUserService = currentUserService;
        }

        public async Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user is null)
            {
                throw new CustomException($"No Accounts Registered with {request.UserName}.");
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                throw new CustomException($"Invalid Credentials for '{request.UserName}'.");
            }

            var response = await GetAuthResponse(user);
            return new ApiResponse<AuthResponse>(response, $"Authenticated {user.UserName}");
        }

        private async Task<AuthResponse> GetAuthResponse(ApplicationUser user)
        {
            JwtSecurityToken jwtSecurityToken = await GenerateJwtToken(user);

            AuthResponse response = new AuthResponse();
            response.Id = user.Id;
            response.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            response.EmailAddress = user.Email;
            response.UserName = user.UserName;
            return response;
        }

        public async Task<bool> ChangeUserEmail(string userId, string email)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var emailChangeToken = await _userManager.GenerateChangeEmailTokenAsync(user, email);
            var changeEmailResponse = await _userManager.ChangeEmailAsync(user, email, emailChangeToken);
            if (changeEmailResponse.Succeeded)
            {
                return true;
            }
            throw new CustomException("Can't change user email");
        }

        public async Task<bool> ChangeUserPhone(string userId, string phone)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var phoneChangeToken = await _userManager.GenerateChangePhoneNumberTokenAsync(user, phone);
            var changePhoneResponse = await _userManager.ChangePhoneNumberAsync(user, phone, phoneChangeToken);
            if (changePhoneResponse.Succeeded)
            {
                return true;
            }
            throw new CustomException("Can't change user phone");
        }

        public async Task<bool> ChangeUserName(string userId, string name)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var changeNameResponse = await _userManager.SetUserNameAsync(user, name);
            if (changeNameResponse.Succeeded)
            {
                return true;
            }
            throw new CustomException("Can't change user name");
        }

        public async Task<ApiResponse<string>> RegisterAsync(SignUpRequest request)
        {
            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                throw new CustomException($"Username '{request.UserName}' is already taken.");
            }
            var user = new ApplicationUser
            {
                Email = request.EmailAddress,
                UserName = request.UserName,
                PhoneNumber = request.UserName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };
            var userWithSameEmail = await _userManager.FindByEmailAsync(request.EmailAddress);
            if (userWithSameEmail == null)
            {
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    return new ApiResponse<string>(user.Id, message: $"User Registered.");
                }

                throw new CustomException($"{result.Errors}");
            }

            throw new CustomException($"Email {request.EmailAddress } is already registered.");
        }

        public async Task ForgotPassword(ForgotPasswordRequest model)
        {
            var account = await _userManager.FindByEmailAsync(model.Email);
            if (account is null) return;
            var passwordResetUri = await SendResetPasswordEmail(account);

            if (!string.IsNullOrWhiteSpace(passwordResetUri))
            {
                if (model.Email != null)
                    await _mailService.SendEmail(model.Email, account.UserName, "Reset Password",
                        $"You reset token is - {passwordResetUri}");
            }
        }

        public async Task<ApiResponse<string>> ResetPassword(ResetPasswordRequest model)
        {
            try
            {
                var account = await _userManager.FindByEmailAsync(model.EmailAddress);
                if (account is null) throw new CustomException($"No Accounts Registered with {model.EmailAddress}.");

                var tokenHistory = await _resetPasswordRepository.GetAsync(c =>
                    c.EmailAddress == model.EmailAddress && c.ResetOtp == model.ResetPasswordToken);
                if(tokenHistory is null) throw new CustomException("invalid email or token, Please try again!");

                var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(tokenHistory.ResetToken));

                var result = await _userManager.ResetPasswordAsync(account, token, model.NewPassword);
                if (result.Succeeded)
                {
                    return new ApiResponse<string>(model.EmailAddress!, message: $"Password Reset Successful.");
                }
                throw new CustomException($"Error occured while reseting the password.");
            }
            catch (Exception e)
            {
                throw new CustomException($"Error occured while reseting the password. {e.Message}");
            }
        }

        public async Task<ApiResponse<string>> ChangePassword(ChangePwdRequest model)
        {
            try
            {
                var account = await _userManager.FindByEmailAsync(model.EmailAddress);
                if (account is null) throw new CustomException($"No Accounts Registered with {model.EmailAddress}.");

                var checkOldPassword = await _signInManager.PasswordSignInAsync(account.UserName, model.CurrentPassword, false, lockoutOnFailure: false);
                if (!checkOldPassword.Succeeded)
                {
                    throw new CustomException($"Invalid Credentials for '{account.Email}'.");
                }

                var result = await _userManager.ChangePasswordAsync(account, model.CurrentPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    return new ApiResponse<string>(account.Email, "Password changed");
                }
                throw new CustomException($"Error occured while change the password.");
            }
            catch (Exception e)
            {
                throw new CustomException($"Error occured while change the password. {e.Message}");
            }
        }

        public async Task<bool> CheckUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> CheckUserByPhone(string phoneNumber)
        {
            var user = await _userManager.FindByNameAsync(phoneNumber);
            if (user != null)
            {
                return true;
            }

            return false;
        }

        public async Task<ApplicationUser> GetLoggedInUser()
        {
            var email = _currentUserService.Email;
            var user = await _userManager.FindByEmailAsync(email);
            return user;
        }
        
        #region Helper Method

        public async Task<IEnumerable<Claim>> GetUserClaims(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = roles.Select(role => new Claim("roles", role)).ToList();

            var claims = new[]
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Sid, Guid.NewGuid().ToString()),
                }
                .Union(userClaims)
                .Union(roleClaims);

            return claims;
        }

        private async Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role));
            }

            var claims = new[]
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
                    new Claim(ClaimTypes.Sid, Guid.NewGuid().ToString()),
                }
                .Union(userClaims)
                .Union(roleClaims);

                var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key!));
                var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                    signingCredentials: signingCredentials);

                return jwtSecurityToken;
        }

        private async Task<string> SendResetPasswordEmail(ApplicationUser user)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            Random generator = new Random();
            string otpCode = generator.Next(0, 100000).ToString("D6");

            var tokenHistory = new ResetPasswordTokenHistory(user.Email, user.PhoneNumber, user.UserName, code, otpCode);

            if (await _resetPasswordRepository.AddAsync(tokenHistory)) return tokenHistory.ResetOtp;

            return string.Empty;
        }

        #endregion
    }
}
