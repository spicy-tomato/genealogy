using Genealogy.Domain.Models;
using Genealogy.Infrastructure.Repositories.Abstractions;
using Microsoft.Extensions.Logging;
using Neo4j.Driver;

namespace Genealogy.Infrastructure.Repositories.Implementations;

internal class PersonRepository(IDriver driver, ILogger<PersonRepository> logger) : IPersonRepository
{
    public async Task<Guid?> Add(Person person)
    {
        const string textQuery =
            """
                CREATE (p:Person {id: $id, name: $name, birthDate: $birthDate})
                RETURN p
            """;

        Query query = new(textQuery, new
        {
            id = person.Id,
            name = person.Name,
            birthDate = person.BirthDate
        });

        await using IAsyncSession? session = driver.AsyncSession();
        try
        {
            IResultCursor? result = await session.RunAsync(query);

            // Get the created node from the result
            IRecord? record = await result.SingleAsync();
            INode? createdNode = record["p"].As<INode>();

            return Guid.Parse((string)createdNode.Properties["id"]);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error: {Message}", ex.Message);
            return null;
        }
    }
}