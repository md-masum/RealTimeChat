using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System.Security.Claims;
using Ui.Models;

namespace Ui.Pages
{
    public partial class Message
    {
        [CascadingParameter] public HubConnection? HubConnection { get; set; }
        private string? receverId;
        private string? messageInput;
        private List<string> Messages = new List<string>();
        public List<UserDto>? UserList { get; set; }

        protected override async Task OnInitializedAsync()
        {
            // hubConnection = new HubConnectionBuilder()
            //     .WithUrl(NavigationManager.ToAbsoluteUri("/chathub"))
            //     .Build();

            // hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            // {
            //     var encodedMsg = $"{user}: {message}";
            //     messages.Add(encodedMsg);
            //     StateHasChanged();
            // });

            // await hubConnection.StartAsync();
            var users = await _chatManager.GetUsersAsync();
            if (users.Data != null) UserList = users.Data;

            HubConnection ??= new HubConnectionBuilder()
                .WithUrl("https://localhost:7280/chathub",
                    options => { options.AccessTokenProvider = async () => await _authenticationService.GetAccessToken(); })
                .Build();
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }

            HubConnection.On<string, string, string>("ReceiveMessage", (sender, recever, message) =>
            {
                var encodedMsg = $"{sender} - {recever}: {message}";
                Messages.Add(encodedMsg);
                StateHasChanged();
            });
        }

        private async Task InfoBtnOnClick()
        {
            await _toastService.ShowInfo("There was a problem with your network connection.", 50000);
        }
        private async Task WarnBtnOnClick()
        {
            await _toastService.ShowWarn("There was a problem with your network connection.", 50000);
        }
        private async Task SuccessBtnOnClick()
        {
            await _toastService.ShowSuccess("There was a problem with your network connection.", 50000);
        }
        private async Task ErrorBtnOnClick()
        {
            await _toastService.ShowError("There was a problem with your network connection.", 50000);
        }

        private async Task Send()
        {
            var state = await _authStateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            if (HubConnection is not null)
            {
                await HubConnection.SendAsync("SendMessage", user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault(), receverId, messageInput);
            }
        }

        public bool IsConnected =>
            HubConnection?.State == HubConnectionState.Connected;
    }
}
