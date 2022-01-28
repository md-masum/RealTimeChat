using Microsoft.AspNetCore.SignalR.Client;

namespace Ui.Store
{
    public class StoreContainer
    {
        private string _savedString;

        public string Property
        {
            get => _savedString ?? string.Empty;
            set
            {
                _savedString = value;
                NotifyStateChanged();
            }
        }

        public HubConnection HubConnection { get; set; }

        public event Action OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
