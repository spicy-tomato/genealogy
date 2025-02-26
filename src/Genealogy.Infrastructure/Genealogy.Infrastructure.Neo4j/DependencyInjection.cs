using Genealogy.Infrastructure.Neo4j.Repositories.Abstractions;
using Genealogy.Infrastructure.Neo4j.Repositories.Implementations;
using Genealogy.Infrastructure.Neo4j.Services.Abstractions;
using Genealogy.Infrastructure.Neo4j.Services.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Neo4jClient;
using Newtonsoft.Json.Serialization;

namespace Genealogy.Infrastructure.Neo4j;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddNeo4J(this WebApplicationBuilder builder)
    {
        string? uri = builder.Configuration["Connections:Neo4j:Uri"];
        string? username = builder.Configuration["Connections:Neo4j:Username"];
        string? password = builder.Configuration["Connections:Neo4j:Password"];
        
        BoltGraphClient client = new(uri, username, password);
        client.JsonContractResolver = new CamelCasePropertyNamesContractResolver();
        client.ConnectAsync();
        
        builder.Services.AddSingleton(client);

        return builder;
    }

    public static WebApplicationBuilder AddNeo4JRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IFamilyRepository, FamilyRepository>();
        builder.Services.AddScoped<IPersonRepository, PersonRepository>();

        builder.Services.AddScoped<IFamilyService, FamilyService>();

        return builder;
    }
}