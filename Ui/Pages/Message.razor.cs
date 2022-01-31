using Microsoft.AspNetCore.SignalR.Client;
using Syncfusion.Blazor.Inputs;
using Ui.Models;

namespace Ui.Pages
{
    public partial class Message
    {
        public List<UserDto> UserList { get; set; }
        public List<ChatMessage> ChatMessage { get; set; }
        public UserDto Receiver { get; set; }

        private SfTextBox _sfTextBox;

        protected override async Task OnInitializedAsync()
        {
            var users = await _httpClient.GetAsync<List<UserDto>>("api/user/Users");
            if (users != null && users.Any()) UserList = users;

            _store.HubConnection ??= new HubConnectionBuilder()
                .WithUrl("https://localhost:7280/chathub",
                    options => { options.AccessTokenProvider = async () => await _authenticationService.GetAccessToken(); })
                .Build();
            if (_store.HubConnection.State == HubConnectionState.Disconnected)
            {
                await _store.HubConnection.StartAsync();
            }

            _store.HubConnection.On<ChatMessage>("ReceiveMessage", (message) =>
            {
                ChatMessage?.Add(message);
                StateHasChanged();
            });
        }

        private async Task OnSelect(UserDto selectedUser)
        {
            Receiver = selectedUser;
            if (Receiver is null) return;
            var messages = await _httpClient.GetAsync<List<ChatMessage>>($"api/chat/{Receiver.Id}");
            if (messages != null && messages.Any()) ChatMessage = messages;
        }

        private async Task OnSend()
        {
            if (_store.HubConnection is not null)
            {
                var saveMessage = new SaveOrUpdateMessage
                {
                    Message = _sfTextBox?.Value,
                    ToUserId = Receiver?.Id
                };
                var chatMessage = await _httpClient.PostAsync<List<ChatMessage>, SaveOrUpdateMessage>("api/chat", saveMessage);
                ChatMessage = chatMessage;
                StateHasChanged();
            }
        }

        public bool IsConnected =>
            _store.HubConnection?.State == HubConnectionState.Connected;
    }
}
