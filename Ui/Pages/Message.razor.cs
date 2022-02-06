using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using Ui.Models;
using Ui.Shared;

namespace Ui.Pages
{
    public partial class Message
    {
        private DotNetObjectReference<Message> _objRef;

        private bool _isLoading = false;
        public List<UserDto> UserList { get; set; }
        public List<ChatMessage> ChatMessage { get; set; }
        public UserDto Receiver { get; set; }

        private string _sfTextBox;

        protected override async Task OnInitializedAsync()
        {
            _isLoading = true;
            var users = await _httpClient.GetAsync<List<UserDto>>("api/user/Users");
            if (users != null && users.Any()) UserList = users;
            _isLoading = false;

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

            _store.HubConnection.On<string, string, string>("RtcClientProtocol", async (data, sender, receiver) =>
            {
                Console.WriteLine(data, sender, receiver);
                _objRef = DotNetObjectReference.Create(this);
                var senderId = await _authenticationService.GetCurrentUserId();
                var receiverId = Receiver.Id;
                if (senderId != null && receiverId != null && senderId == sender && receiverId == receiver)
                {
                    await _js.InvokeAsync<string>(JsInteropConstant.HandleSignallingData, data, _objRef);
                }
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

        protected override void OnAfterRender(bool firstRender)
        {
            _js.InvokeVoid(JsInteropConstant.ScrollToBottom, "listOfMessage");
        }

        private async Task OnSend()
        {
            var saveMessage = new SaveOrUpdateMessage
            {
                Message = _sfTextBox,
                ToUserId = Receiver?.Id
            };
            var chatMessage = await _httpClient.PostAsync<List<ChatMessage>, SaveOrUpdateMessage>("api/chat", saveMessage);
            ChatMessage = chatMessage;
            _sfTextBox = "";
            StateHasChanged();
        }

        //video call feature
        [JSInvokable]
        public async Task SendSignalRData(string data)
        {
            var sendData = new
            {
                Sender = await _authenticationService.GetCurrentUserId(),
                Receiver = Receiver.Id,
                Data = data
            };
            await _httpClient.SignalRPost("api/chat/RtcClientProtocol", sendData);
        }
        private async Task OnVideoCall()
        {
            var receiver = Receiver.Id;

            _objRef = DotNetObjectReference.Create(this);
            await _js.InvokeAsync<string>(JsInteropConstant.StartCall, receiver, _objRef);
        }

        private void OnAudioCall()
        {

        }

        private void MuteVideo()
        {

        }

        private void MuteAudio()
        {

        }
        public bool IsConnected =>
            _store.HubConnection?.State == HubConnectionState.Connected;
    }
}
