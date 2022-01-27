using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Syncfusion.Blazor.Inputs;
using Ui.Models;

namespace Ui.Pages
{
    public partial class Message
    {
        [CascadingParameter] public HubConnection HubConnection { get; set; }
        public List<UserDto> UserList { get; set; }
        public List<ChatMessage> ChatMessage { get; set; }
        public UserDto Receiver { get; set; }

        private SfTextBox _sfTextBox;

        protected override async Task OnInitializedAsync()
        {
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

            HubConnection.On<ChatMessage>("ReceiveMessage", (message) =>
            {
                ChatMessage?.Add(message);
                StateHasChanged();
            });
        }

        private async Task OnSelect(UserDto selectedUser)
        {
            Receiver = selectedUser;
            if (Receiver is null) return;
            var messages = await _chatManager.GetConversationAsync(Receiver.Id);
            if (messages.Data != null) ChatMessage = messages.Data;
        }

        private async Task OnMessageSelect(ChatMessage chatMessage)
        {

        }

        private async Task OnSend()
        {
            var state = await _authStateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            if (HubConnection is not null)
            {
                var saveMessage = new SaveOrUpdateMessage
                {
                    Message = _sfTextBox?.Value,
                    ToUserId = Receiver?.Id
                };
                var chatMessage = await _chatManager.SaveMessageAsync(saveMessage);
                ChatMessage = chatMessage.Data;
                StateHasChanged();
            }
        }

        public bool IsConnected =>
            HubConnection?.State == HubConnectionState.Connected;
    }
}
