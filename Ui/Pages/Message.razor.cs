using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System.Security.Claims;
using Ui.HttpRepository;
using Ui.Models;

namespace Ui.Pages
{
    public partial class Message
    {
        [CascadingParameter] public HubConnection? HubConnection { get; set; }
        private string? receverId;
        private string? messageInput;
        private List<string> messages = new List<string>();
        private List<UserDto> userList = new List<UserDto>();

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
            userList.AddRange(users.Data);

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
                messages.Add(encodedMsg);
                StateHasChanged();
            });
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

        public async ValueTask DisposeAsync()
        {
            if (HubConnection is not null)
            {
                await HubConnection.DisposeAsync();
            }
        }
    }
}
