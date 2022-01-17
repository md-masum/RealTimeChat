using Microsoft.AspNetCore.Components;
using Ui.HttpRepository;
using Ui.Models.Auth.Request;

namespace Ui.Pages
{
    public partial class Login
    {
        private readonly LoginRequest _userForAuthentication = new LoginRequest();

        [Inject]
        public IAuthenticationService? AuthenticationService { get; set; }
        [Inject]
        public NavigationManager? NavigationManager { get; set; }
        public bool ShowAuthError { get; set; }
        public string? Error { get; set; }

        public async Task ExecuteLogin()
        {
            ShowAuthError = false;

            var result = await AuthenticationService!.Login(_userForAuthentication);
            if (!result.IsSuccess)
            {
                Error = result.Message;
                ShowAuthError = true;
            }
            else
            {
                NavigationManager!.NavigateTo("/");
            }
        }
    }
}
