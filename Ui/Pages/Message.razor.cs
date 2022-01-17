using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Ui.HttpRepository;

namespace Ui.Pages
{
    public partial class Message
    {
        private readonly ILocalStorageService _localStorage;
        private HubConnection? hubConnection;
        private List<string> messages = new List<string>();
        private string? userInput;
        private string? messageInput;
        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            
            hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7280/chathub", options =>
                {
                    options.AccessTokenProvider = async () => await AuthenticationService.GetAccessToken();
                })
                .Build();

            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                var encodedMsg = $"{user}: {message}";
                messages.Add(encodedMsg);
                StateHasChanged();
            });

            await hubConnection.StartAsync();
        }

        private async Task Send()
        {
            if (hubConnection is not null)
            {
                await hubConnection.SendAsync("SendMessage", userInput, messageInput);
            }
        }

        public bool IsConnected =>
            hubConnection?.State == HubConnectionState.Connected;

        public async ValueTask DisposeAsync()
        {
            if (hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }
        }
    }
}
