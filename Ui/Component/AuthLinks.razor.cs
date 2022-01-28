namespace Ui.Component
{
    public partial class AuthLinks
    {
        public string UserName { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (await _authenticationService.IsUserAuthenticated())
            {
                UserName = await _authenticationService.GetCurrentUserName();
            }
        }
    }
}
