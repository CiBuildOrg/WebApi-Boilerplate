namespace App.Exceptions
{
    public class RegistrationException : AbstractRequestException
    {
        public RegistrationException(int code, string message) : base(code, message)
        {
        }

        public RegistrationException(int code, string message, string dataDump) : base(code, message, dataDump)
        {
        }
    }
}