using System.Net;
using System.Text.Json;
using FluentValidation;
using Genealogy.Application.Models;

namespace Genealogy.API.Middlewares;

public class GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/json";

            if (ex is ValidationException validationException)
            {
                await HandleValidationExceptionAsync(context, validationException);
            }
            else
            {
                await HandleGlobalExceptionAsync(context, ex);
            }
        }
    }

    private static Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        Response<bool> response = Response.ValidationError(exception);
        string result = JsonSerializer.Serialize(response);

        return context.Response.WriteAsync(result);
    }

    private Task HandleGlobalExceptionAsync(HttpContext context, Exception exception)
    {
        logger.LogError(exception, "Unhandled error");

        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        Response<bool> response = Response.Error("Internal Server Error");
        string result = JsonSerializer.Serialize(response);

        return context.Response.WriteAsync(result);
    }
}