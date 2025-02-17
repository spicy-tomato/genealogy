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
        await using IAsyncSession? session = driver.AsyncSession();
        IAsyncTransaction transaction = await session.BeginTransactionAsync();

        try
        {
            string personId = await Create(person, transaction);

            foreach ((string otherPersonId, PersonRelationshipDetails relationship) in person.Relationships)
            {
                await Connect(personId, relationship.Relationship, otherPersonId, relationship.ReversedRelationship,
                    transaction);
            }

            await transaction.CommitAsync();

            return personId;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error: {Message}", ex.Message);

            await transaction.RollbackAsync();

            return null;
        }
    }

    public async Task<KeyValuePair<string, string>?> Connect(string personId1, Relationship relationship,
        string personId2, Relationship reversedRelationship)
    {
        string relationshipType1 = relationship.ToString().ToUpper() + "_OF";
        string relationshipType2 = reversedRelationship.ToString().ToUpper() + "_OF";

        // e.g.
        // MATCH (husband:Person {id: $husbandId}), (wife:Person {id: $wifeId})
        // MERGE (husband)-[:HUSBAND_OF]->(wife)
        // MERGE (wife)-[:WIFE_OF]->(husband)
        // RETURN husband, wife
        var textQuery =
            $$"""
              MATCH ({{relationship}}:Person {id: $personId1}), ({{reversedRelationship}}:Person {id: $personId2})
              MERGE ({{relationship}})-[:{{relationshipType1}}]->({{reversedRelationship}})
              MERGE ({{reversedRelationship}})-[:{{relationshipType2}}]->({{relationship}})
              RETURN {{relationship}}, {{reversedRelationship}}
              """;

        Query query = new(textQuery, new { personId1, personId2 });

        await using IAsyncSession? session = driver.AsyncSession();
        try
        {
            IResultCursor result = await session.RunAsync(query);
            IRecord record = await result.SingleAsync();
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

    public async Task<bool> Delete(string id)
    {
        await using IAsyncSession? session = driver.AsyncSession();
        IAsyncTransaction? tx = await session.BeginTransactionAsync();

        try
        {
            const string textQuery =
                """
                MATCH (p:Person {id: $id})
                WITH p, count(p) as cnt
                DETACH DELETE p
                RETURN cnt as count
                """;

            Query query = new(textQuery, new { id });

            IResultCursor result = await tx.RunAsync(query);
            IRecord record = await result.SingleAsync();
            var count = record["count"].As<int>();

            if (count == 0)
            {
                await tx.RollbackAsync();
            }
            else
            {
                await tx.CommitAsync();
            }

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error: {Message}", ex.Message);

            await tx.RollbackAsync();

            return false;
        }
        finally
        {
            await session.CloseAsync();
        }
    }

    private static async Task<string> Create(Person person, IAsyncTransaction transaction)
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

        IResultCursor result = await transaction.RunAsync(query);
        IRecord record = await result.SingleAsync();
        INode? createdNode = record["p"].As<INode>();

        return (string)createdNode.Properties["id"];
    }

    private static async Task Connect(string personId1, Relationship relationship, string personId2,
        Relationship reversedRelationship, IAsyncTransaction transaction)
    {
        string relationshipType = relationship.ToString().ToUpper() + "_OF";
        string reversedRelationshipType = reversedRelationship.ToString().ToUpper() + "_OF";

        // e.g.
        // MATCH (husband:Person {id: $husbandId}), (wife:Person {id: $wifeId})
        // MERGE (husband)-[:HUSBAND_OF]->(wife)
        // MERGE (wife)-[:WIFE_OF]->(husband)
        // RETURN husband, wife
        var textQuery =
            $$"""
              MATCH ({{relationship}}:Person {id: $personId1}), ({{reversedRelationship}}:Person {id: $personId2})
              MERGE ({{relationship}})-[:{{relationshipType}}]->({{reversedRelationship}})
              MERGE ({{reversedRelationship}})-[:{{reversedRelationshipType}}]->({{relationship}})
              """;

        Query query = new(textQuery, new { personId1, personId2 });

        await transaction.RunAsync(query);
    }
}