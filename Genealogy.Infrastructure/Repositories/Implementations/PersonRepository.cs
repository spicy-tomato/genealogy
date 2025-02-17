using Genealogy.Domain.Enums;
using Genealogy.Domain.Models;
using Genealogy.Infrastructure.Repositories.Abstractions;
using Microsoft.Extensions.Logging;
using Neo4j.Driver;

namespace Genealogy.Infrastructure.Repositories.Implementations;

internal class PersonRepository(IDriver driver, ILogger<PersonRepository> logger) : IPersonRepository
{
    public async Task<string?> Add(Person person)
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
            IRecord? record = await result.SingleAsync();
            INode? createdNode = record["p"].As<INode>();

            return (string)createdNode.Properties["id"];
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error: {Message}", ex.Message);
            return null;
        }
    }

    public async Task<KeyValuePair<string, string>?> Connect(string personId1, Relationship relationship1,
        string personId2, Relationship relationship2)
    {
        string relationshipType1 = relationship1.ToString().ToUpper() + "_OF";
        string relationshipType2 = relationship2.ToString().ToUpper() + "_OF";

        // e.g.
        // MATCH (husband:Person {id: $husbandId}), (wife:Person {id: $wifeId})
        // MERGE (husband)-[:HUSBAND_OF]->(wife)
        // MERGE (wife)-[:WIFE_OF]->(husband)
        // RETURN husband, wife
        var textQuery =
            $$"""
              MATCH ({{relationship1}}:Person {id: $personId1}), ({{relationship2}}:Person {id: $personId2})
              MERGE ({{relationship1}})-[:{{relationshipType1}}]->({{relationship2}})
              MERGE ({{relationship2}})-[:{{relationshipType2}}]->({{relationship1}})
              RETURN {{relationship1}}, {{relationship2}}
              """;

        Query query = new(textQuery, new { personId1, personId2 });

        await using IAsyncSession? session = driver.AsyncSession();
        try
        {
            IResultCursor? result = await session.RunAsync(query);
            IRecord? record = await result.SingleAsync();
            INode? husband = record["Husband"].As<INode>();
            INode? wife = record["Wife"].As<INode>();

            return new KeyValuePair<string, string>((string)husband.Properties["id"], (string)wife.Properties["id"]);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error: {Message}", ex.Message);
            return null;
        }
    }
}