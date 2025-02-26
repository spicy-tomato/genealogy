using Genealogy.Infrastructure.Exceptions;
using JetBrains.Annotations;

namespace Genealogy.API.OpenApi;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public abstract class ErrorResponse
{
    public bool Data { get; init; }
    public string Message { get; init; } = "Error";
    public bool Success { get; init; }
    public IEnumerable<Error>? Errors { get; init; }
}