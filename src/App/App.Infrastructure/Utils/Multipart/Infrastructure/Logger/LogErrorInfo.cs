namespace App.Infrastructure.Utils.Multipart.Infrastructure.Logger
{
    public class LogErrorInfo
    {
        public string ErrorMessage { get; private set; }
        public System.Exception Exception { get; private set; }
        public bool IsException { get; private set; }

        public LogErrorInfo(string errorMessage)
        {
            ErrorMessage = errorMessage;
            IsException = false;
        }

        public LogErrorInfo(System.Exception exception)
        {
            Exception = exception;
            IsException = true;
        }
    }
}