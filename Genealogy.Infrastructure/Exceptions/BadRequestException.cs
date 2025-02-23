using Microsoft.AspNetCore.Http;

namespace Genealogy.Infrastructure.Exceptions;

public class BadRequestException : UserFriendlyException
{
    private BadRequestException(string message) : base(message, StatusCode)
    {
    }

    private new const int StatusCode = StatusCodes.Status400BadRequest;

    public static BadRequestException Create(string message)
    {
        return new BadRequestException(message);
    }
}