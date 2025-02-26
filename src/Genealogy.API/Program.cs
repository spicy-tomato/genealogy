using System.ComponentModel;
using System.Text.Json;
using Genealogy.API.Auth;
using Genealogy.API.Middlewares;
using Genealogy.API.OpenApi;
using Genealogy.Application;
using Genealogy.Application.Models;
using Genealogy.Application.UseCases.Families.Commands.Update;
using Genealogy.Application.UseCases.People.Commands.Create;
using Genealogy.Application.UseCases.People.Commands.Delete;
using Genealogy.Application.UseCases.People.Queries.GetRelatedByPersonId;
using Genealogy.Infrastructure;
using Genealogy.Infrastructure.Dtos.People;
using MediatR;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder
    // API
    .AddAuth()
    // Infrastructure
    .AddNeo4J()
    .AddRepositories()
    // Application
    .AddMediatR()
    .AddFluentValidation();

builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

builder.Services.AddOpenApi();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Theme = ScalarTheme.BluePlanet;
        options.Layout = ScalarLayout.Modern;
        options.EnabledClients =
        [
            ScalarClient.HttpClient, ScalarClient.RestSharp, ScalarClient.Http, ScalarClient.JQuery, ScalarClient.Curl
        ];
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.MapGet("person", async (ISender sender, string? rootId, int? depth) =>
    {
        GetRelatedByPersonIdQuery command = new(rootId, depth ?? 5);
        Response<GetRelatedPersonResult> result = await sender.Send(command);

        return result;
    })
    .ProducesOk<string>()
    .ProducesBadRequest()
    .WithDescription("Get people by root");

app.MapPost("person", async (ISender sender, CreatePersonRequest request) =>
    {
        CreatePersonCommand command = new(request.Name, request.BirthDate, request.Relationship,
            request.AnotherPersonIds);
        Response<string> result = await sender.Send(command);

        return result;
    })
    .ProducesOk<string>()
    .ProducesBadRequest()
    .WithDescription("Add a person");

app.MapDelete("person/{id}", async ([Description("Person ID to delete")] string id, ISender sender) =>
    {
        DeletePersonCommand command = new(id);
        Response<bool> result = await sender.Send(command);

        return result;
    })
    .WithDescription("Delete a person")
    .Produces(StatusCodes.Status400BadRequest);

app.MapPatch("family", async (ISender sender, UpdateFamilyRequest request) =>
    {
        UpdateFamilyCommand command = new(request.Person1, request.Person2, request.Family);
        Response<bool> result = await sender.Send(command);

        return result;
    })
    .WithDescription("Connect two existing people");

app.Run();