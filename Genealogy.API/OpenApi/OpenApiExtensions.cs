namespace Genealogy.API.OpenApi;

public static class OpenApiExtensions
{
    public static RouteHandlerBuilder ProducesOk<T>(this RouteHandlerBuilder builder)
    {
        return builder.Produces<SuccessResponse<T>>();
    }

    public static RouteHandlerBuilder ProducesBadRequest(this RouteHandlerBuilder builder)
    {
        return builder.Produces<ErrorResponse>(StatusCodes.Status400BadRequest);
    }
}