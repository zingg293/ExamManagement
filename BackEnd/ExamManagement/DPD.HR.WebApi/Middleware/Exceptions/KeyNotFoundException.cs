namespace DPD.HR.Infrastructure.WebApi.Middleware.Exceptions
{
    public class KeyNotFoundException : Exception
    {
        public KeyNotFoundException(string message) : base(message)
        { }
    }
}
