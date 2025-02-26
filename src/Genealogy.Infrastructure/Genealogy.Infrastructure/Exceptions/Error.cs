using JetBrains.Annotations;

namespace Genealogy.Infrastructure.Exceptions;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class Error(string message, object? details = null)
{
    public string Message { get; init; } = message;
    public object? Details { get; init; } = details;
}