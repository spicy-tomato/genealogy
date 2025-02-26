using JetBrains.Annotations;

namespace Genealogy.API.OpenApi;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public abstract class SuccessResponse<T>
{
    public T Data { get; init; } = default!;
    public string Message { get; init; } = "Success";
    public bool Success { get; init; } = true;
}