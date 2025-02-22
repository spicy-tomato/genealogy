using Microsoft.AspNetCore.Http;

namespace Genealogy.Infrastructure.Exceptions;

public abstract class UserFriendlyException(string message, IEnumerable<Error>? error = null) : Exception(message)
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public IEnumerable<Error>? Error { get; } = error;
}