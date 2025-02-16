using Genealogy.Infrastructure.Repositories.Abstractions;
using Genealogy.Infrastructure.Repositories.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Neo4j.Driver;

namespace Genealogy.Infrastructure;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddNeo4J(this WebApplicationBuilder builder)
    {
        string? uri = builder.Configuration["Neo4j:Uri"];
        string? username = builder.Configuration["Neo4j:Username"];
        string? password = builder.Configuration["Neo4j:Password"];

        IDriver? driver = GraphDatabase.Driver(uri, AuthTokens.Basic(username, password));
        if (driver == null)
        {
            throw new InvalidOperationException("Neo4j authentication failed.");
        }

        builder.Services.AddSingleton(driver);

        return builder;
    }

    public static WebApplicationBuilder AddRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IPersonRepository, PersonRepository>();

        return builder;
    }
}