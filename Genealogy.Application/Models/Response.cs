using FluentValidation;
using Genealogy.Infrastructure.Exceptions;
using JetBrains.Annotations;

namespace Genealogy.Application.Models;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class Response<T>
{
    public T? Data { get; init; }
    public string Message { get; init; }
    public bool Success { get; init; }
    public IEnumerable<Error>? Errors { get; init; }

    internal Response(T data, string? message)
    {
        Data = data;
        Message = message ?? "Success";
        Success = true;
    }

    internal Response(string message, IEnumerable<Error>? errors = null)
    {
        Message = message;
        Success = false;
        Errors = errors;
    }
}

public static class Response
{
    public static Response<bool> Error(string message, IEnumerable<Error>? errors = null)
    {
        return new Response<bool>(message, errors);
    }

    public static Response<bool> ValidationError(ValidationException exception)
    {
        IEnumerable<Error> errors = exception.Errors.Select(e => new Error(e.ErrorMessage));
        return new Response<bool>("Validation error", errors);
    }

    public static Response<T> Succeed<T>(T data, string? message = null)
    {
        return new Response<T>(data, message);
    }

    public static Response<T> Error<T>(string message, IEnumerable<Error>? errors = null)
    {
        return new Response<T>(message, errors);
    }

    // public static Response<T> NotFound<T>(string value, string field="ID")
    // {
    //     return new Response<T>($"{typeof(T).Name} with {field}={value} was not found.");
    // }
}