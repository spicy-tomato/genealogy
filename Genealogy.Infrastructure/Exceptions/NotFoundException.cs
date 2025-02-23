using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;

namespace Genealogy.Infrastructure.Exceptions;

public class NotFoundException : UserFriendlyException
{
    private NotFoundException(Error error) : base(_message, StatusCode, [error])
    {
    }

    private NotFoundException(string message) : base(message, StatusCode)
    {
    }

    private const string _message = "The requested resource was not found.";

    private new const int StatusCode = StatusCodes.Status404NotFound;

    public static NotFoundException Create(string message)
    {
        return new NotFoundException(message);
    }

    [UsedImplicitly]
    public static NotFoundException WithId<T>(string id)
    {
        return new NotFoundException(CreateInnerError<T>([id]));
    }

    public static NotFoundException WithId<T>(IEnumerable<string> id)
    {
        return new NotFoundException(CreateInnerError<T>(id));
    }

    private static Error CreateInnerError<T>(IEnumerable<string> ids)
    {
        var message = $"{typeof(T).Name} with ID(s) was not found";
        return new Error(message, ids);
    }
}