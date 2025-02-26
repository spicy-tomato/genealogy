using Genealogy.Infrastructure.Postgres.Options;
using Genealogy.Infrastructure.Postgres.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Genealogy.Infrastructure.Postgres;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddPostgres(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<PgDbContext>(options =>
        {
            NpgsqlConnectionStringBuilder connection = new()
            {
                Host = builder.Configuration["Connections:Postgres:Host"],
                Port = int.Parse(builder.Configuration["Connections:Postgres:Port"]!),
                Username = builder.Configuration["Connections:Postgres:Username"],
                Password = builder.Configuration["Connections:Postgres:Password"]
            };
            options.UseNpgsql(connection.ConnectionString);
        });

        builder.Services.Configure<WhiteList>(builder.Configuration.GetSection("WhiteList"));

        return builder;
    }
}