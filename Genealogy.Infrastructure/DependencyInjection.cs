using Genealogy.Infrastructure.Repositories.Abstractions;
using Genealogy.Infrastructure.Repositories.Implementations;
using Genealogy.Infrastructure.Services.Abstractions;
using Genealogy.Infrastructure.Services.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Neo4j.Driver;
using Neo4jClient;
using Newtonsoft.Json.Serialization;

namespace Genealogy.Infrastructure;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddNeo4J(this WebApplicationBuilder builder)
    {
        string? uri = builder.Configuration["Neo4j:Uri"];
        string? username = builder.Configuration["Neo4j:Username"];
        string? password = builder.Configuration["Neo4j:Password"];
        
        BoltGraphClient client = new(uri, username, password);
        client.JsonContractResolver = new CamelCasePropertyNamesContractResolver();
        client.ConnectAsync();
        
        builder.Services.AddSingleton(client);

        return builder;
    }

    public static WebApplicationBuilder AddRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IFamilyRepository, FamilyRepository>();
        builder.Services.AddScoped<IPersonRepository, PersonRepository>();

        builder.Services.AddScoped<IFamilyService, FamilyService>();

        return builder;
    }
}