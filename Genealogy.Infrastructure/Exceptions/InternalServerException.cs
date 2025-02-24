using Microsoft.AspNetCore.Http;

namespace Genealogy.Infrastructure.Exceptions;

public class InternalServerException : UserFriendlyException
{
    private InternalServerException(string message) : base(message, StatusCode)
    {
    }

    private new const int StatusCode = StatusCodes.Status500InternalServerError;

    public static InternalServerException Create(string message)
    {
        return new InternalServerException(message);
    }
}