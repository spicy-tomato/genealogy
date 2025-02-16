using JetBrains.Annotations;

namespace Genealogy.Application.Models;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class Error(string message)
{
    public string Message { get; init; } = message;
}