using FluentValidation;
using Genealogy.Application.Models;
using Genealogy.Infrastructure.Exceptions;

namespace Genealogy.API.Middlewares;

public class GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException e)
        {
            await HandleValidationExceptionAsync(context, e);
        }
        catch (Exception e) when (e is UserFriendlyException friendlyException)
        {
            await HandleUserFriendlyExceptionAsync(context, friendlyException);
        }
        catch (Exception e)
        {
            await HandleGlobalExceptionAsync(context, e);
        }
        finally
        {
            context.Response.ContentType ??= "application/json";
        }
    }

    private static Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        Response<bool> response = Response.ValidationError(exception);
        return context.Response.WriteAsJsonAsync(response);
    }

    private static Task HandleUserFriendlyExceptionAsync(HttpContext context, UserFriendlyException exception)
    {
        context.Response.StatusCode = exception.StatusCode ?? StatusCodes.Status400BadRequest;

        Response<bool> response = Response.Error(exception.Message, exception.Error);
        return context.Response.WriteAsJsonAsync(response);
    }

    private Task HandleGlobalExceptionAsync(HttpContext context, Exception exception)
    {
        logger.LogError(exception, "Unhandled error");

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        Response<bool> response = Response.Error("Some errors occurred");
        return context.Response.WriteAsJsonAsync(response);
    }
}