using Microsoft.JSInterop;
using Syncfusion.Blazor.Notifications;
using Ui.Shared;

namespace Ui.Service
{
    public enum ToastPositions
    {
        Center,
        Right,
        Left
    }

    public class ToastService
    {
        private readonly IJSInProcessRuntime _js;
        public ToastService(IJSInProcessRuntime js)
        {
            SfToastObj = new SfToast();
            ToastPosition = ToastPositions.Right.ToString();
            _js = js;
        }

        public SfToast SfToastObj { get; set; }
        public string ToastPosition { get; set; }
        public void ShowInfo(string message, int timeOut)
        {
            var toastModel = new ToastModel
            {
                Title = "Information!",
                Icon = "e-info toast-icons",
                Content = message,
                Timeout = timeOut,
                CssClass = "e-toast-info"
            };
            Show(toastModel);
        }

        public void ShowWarn(string message, int timeOut)
        {
            var toastModel = new ToastModel
            {
                Title = "Warning!",
                Icon = "e-warning toast-icons",
                Content = message,
                Timeout = timeOut,
                CssClass = "e-toast-warning"
            };
            Show(toastModel);
        }

        public void ShowSuccess(string message, int timeOut)
        {
            var toastModel = new ToastModel
            {
                Title = "Success!",
                Icon = "e-success toast-icons",
                Content = message,
                Timeout = timeOut,
                CssClass = "e-toast-success"
            };
            Show(toastModel);
        }

        public void ShowError(string message, int timeOut)
        {
            var toastModel = new ToastModel
            {
                Title = "Error!",
                Icon = "e-error toast-icons",
                Content = message,
                Timeout = timeOut,
                CssClass = "e-toast-danger"
            };
            Show(toastModel);
        }

        private void Show(ToastModel model)
        {
            _js.InvokeVoid(JsInteropConstant.PlatNotification);
            SfToastObj.Show(model);
        }
    }
}
