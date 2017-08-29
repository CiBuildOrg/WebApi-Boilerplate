namespace App.Infrastructure.Utils.Multipart.Infrastructure.Logger
{
    public class LogErrorInfo
    {
        public string ErrorMessage { get; }
        public System.Exception Exception { get; }
        public bool IsException { get; }

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