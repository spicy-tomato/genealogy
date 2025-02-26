namespace Genealogy.API.OpenApi;

public abstract class SuccessResponse<T>
{
    public T Data { get; init; } = default!;
    public string Message { get; init; } = "Success";
    public bool Success { get; init; } = true;
}