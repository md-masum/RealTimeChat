using Ui.Models.Auth.Request;

namespace Ui.Pages.Auth
{
    public partial class Login
    {
        private readonly LoginRequest _userForAuthentication = new LoginRequest();
        public bool ShowAuthError { get; set; }
        public string Error { get; set; }

        public async Task ExecuteLogin()
        {
            ShowAuthError = false;

            var result = await _authenticationService.Login(_userForAuthentication);
            if (!result.IsSuccess)
            {
                Error = result.Message;
                ShowAuthError = true;
            }
            else
            {
                _navigationManager.NavigateTo("/");
            }
        }
    }
}
