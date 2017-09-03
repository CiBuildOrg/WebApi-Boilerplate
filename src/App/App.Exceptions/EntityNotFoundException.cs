namespace App.Exceptions
{
    public class EntityNotFoundException : AbstractRequestException
    {
        public EntityNotFoundException() : base(200, "Entity not found")
        {
        }

        public EntityNotFoundException(int code, string message, string dataDump) : base(code, message, dataDump)
        {
        }
    }
}