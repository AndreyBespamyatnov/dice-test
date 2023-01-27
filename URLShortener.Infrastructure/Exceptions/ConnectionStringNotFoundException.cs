namespace URLShortener.Infrastructure.Exceptions;

public class ConnectionStringNotFoundException : Exception
{
    public ConnectionStringNotFoundException(string message) : base(message)
    {
    }
}