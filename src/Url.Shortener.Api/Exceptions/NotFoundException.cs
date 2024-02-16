namespace Url.Shortener.Api.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string? message = default, Exception? innerException = default) : base(message, innerException)
    { }
}