namespace Core.Entity.Auth
{
    public class ResetPasswordTokenHistory : BaseEntity
    {
        public ResetPasswordTokenHistory(string emailAddress, string? phoneNumber, string userName, string resetToken, string resetOtp)
        {
            EmailAddress = emailAddress;
            PhoneNumber = phoneNumber;
            UserName = userName;
            ResetToken = resetToken;
            ResetOtp = resetOtp;
        }

        public string EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string ResetToken { get; set; }
        public string ResetOtp { get; set; }
    }
}
