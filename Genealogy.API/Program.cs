using System.ComponentModel;
using System.Text.Json;
using Genealogy.API.Middlewares;
using Genealogy.API.OpenApi;
using Genealogy.Application;
using Genealogy.Application.Models;
using Genealogy.Application.UseCases.Families.Update;
using Genealogy.Application.UseCases.People.Create;
using Genealogy.Application.UseCases.People.Delete;
using Genealogy.Infrastructure;
using MediatR;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder
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

app.UseHttpsRedirection();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

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