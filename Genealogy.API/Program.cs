using Genealogy.API.Middlewares;
using Genealogy.Application;
using Genealogy.Application.Models;
using Genealogy.Application.UseCases.People.Create;
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

builder.Services.AddOpenApi();
builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.MapPost("person", async (ISender sender, CreatePersonRequest request) =>
{
    CreatePersonCommand command = new(request.Name, request.BirthDate);
    Response<Guid> result = await sender.Send(command);

    return result;
});

app.Run();