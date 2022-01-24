using Syncfusion.Blazor.Notifications;
using Ui.Models;

namespace Ui.Service
{
    public class ToastService
    {
        public ToastService()
        {
            SfToastObj = new SfToast();
            ToastPosition = "Right";
        }

        public SfToast SfToastObj { get; set; }
        public string ToastPosition { get; set; }
        public async Task ShowInfo(string message, int timeOut)
        {
            var toastModel = new ToastModel
            {
                Title = "Information!",
                Icon = "e-info toast-icons",
                Content = message,
                Timeout = timeOut,
                CssClass = "e-toast-info"
            };
            await SfToastObj.ShowAsync(toastModel);
        }

        public async Task ShowWarn(string message, int timeOut)
        {
            var toastModel = new ToastModel
            {
                Title = "Warning!",
                Icon = "e-warning toast-icons",
                Content = message,
                Timeout = timeOut,
                CssClass = "e-toast-warning"
            };
            await SfToastObj.ShowAsync(toastModel);
        }

        public async Task ShowSuccess(string message, int timeOut)
        {
            var toastModel = new ToastModel
            {
                Title = "Success!",
                Icon = "e-success toast-icons",
                Content = message,
                Timeout = timeOut,
                CssClass = "e-toast-success"
            };
            await SfToastObj.ShowAsync(toastModel);
        }

        public async Task ShowError(string message, int timeOut)
        {
            var toastModel = new ToastModel
            {
                Title = "Error!",
                Icon = "e-error toast-icons",
                Content = message,
                Timeout = timeOut,
                CssClass = "e-toast-danger"
            };
            await SfToastObj.ShowAsync(toastModel);
        }
    }
}
