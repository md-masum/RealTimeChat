using Syncfusion.Blazor.Notifications;

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
        public ToastService()
        {
            SfToastObj = new SfToast();
            ToastPosition = ToastPositions.Right.ToString();
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
            SfToastObj.Show(toastModel);
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
            SfToastObj.Show(toastModel);
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
            SfToastObj.Show(toastModel);
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
            SfToastObj.Show(toastModel);
        }
    }
}
