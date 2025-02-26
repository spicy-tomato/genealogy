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
                Database = builder.Configuration["Connections:Postgres:Database"]
            };
            options.UseNpgsql(connection.ConnectionString);
        });

        return builder;
    }
}