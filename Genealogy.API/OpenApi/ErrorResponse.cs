using Genealogy.Application.Models;

namespace Genealogy.API.OpenApi;

public abstract class ErrorResponse
{
    public bool Data { get; init; } = false;
    public string Message { get; init; } = "Error";
    public bool Success { get; init; } = false;
    public IEnumerable<Error>? Errors { get; init; }
}