using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using Ui.Shared;

namespace Ui.Store
{
    public class StoreContainer
    {
        private readonly IJSInProcessRuntime _js;

        public StoreContainer(IJSInProcessRuntime js)
        {
            _js = js;
            var data = _js.Invoke<string>(JsInteropConstant.GetSessionStorage, nameof(CurrentCount));
            int.TryParse(data, out _currentCount);
        }
        private int _currentCount;

        public int CurrentCount
        {
            get => _currentCount;
            set
            {
                _currentCount = value;
                _js.InvokeVoid(JsInteropConstant.SetSessionStorage, nameof(CurrentCount), value);
                NotifyStateChanged();
            }
        }

        public HubConnection HubConnection { get; set; }

        public event Action OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
