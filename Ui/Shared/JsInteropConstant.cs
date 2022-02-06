namespace Ui.Shared
{
    public static class JsInteropConstant
    {
        private const string FuncPrefix = "realTimeChat";

        public const string GetSessionStorage = $"{FuncPrefix}.getSessionStorage";
        public const string SetSessionStorage = $"{FuncPrefix}.setSessionStorage";
        public const string ScrollToBottom = $"{FuncPrefix}.scrollToBottom";

        public const string PlatNotification = $"{FuncPrefix}.playNotification";
        public const string PlayCall = $"{FuncPrefix}.playCall";
        public const string PushCall = $"{FuncPrefix}.pushCall";

        public const string StartCall = $"{FuncPrefix}.startCall";
        public const string HandleSignallingData = $"{FuncPrefix}.handleSignallingData";
    }
}
