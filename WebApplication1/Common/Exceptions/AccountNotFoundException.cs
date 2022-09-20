namespace WebApplication1.Common.Exceptions;

public class AccountNotFoundException : Exception
{
    public AccountNotFoundException(string? accountId) : base(accountId)
    {
    }

    public AccountNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}