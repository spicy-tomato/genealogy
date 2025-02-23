namespace Genealogy.Infrastructure.Exceptions;

public abstract class UserFriendlyException(string message, int? statusCode, IEnumerable<Error>? error = null) : Exception(message)
{
    public readonly int? StatusCode = statusCode;

    public IEnumerable<Error>? Error { get; } = error;
}