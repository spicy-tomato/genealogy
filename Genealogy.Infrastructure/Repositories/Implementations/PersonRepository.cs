using Genealogy.Domain.Models;
using Genealogy.Infrastructure.Repositories.Abstractions;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace Genealogy.Infrastructure.Repositories.Implementations;

internal class PersonRepository(BoltGraphClient client) : IPersonRepository
{
    public async Task<string> CreateAsync(Person person)
    {
        ICypherFluentQuery query = client.Cypher
            .Create("(p:Person $person)")
            .WithParam("person", person);

        await query.ExecuteWithoutResultsAsync();

        return person.Id;
    }

    public async Task DeleteAsync(string id)
    {
        ICypherFluentQuery query = client.Cypher
            .Match("(person:Person)")
            .Where<Person>(p => p.Id == id)
            .DetachDelete("person");

        await query.ExecuteWithoutResultsAsync();
    }

    public async Task<List<string>?> GetIdsNotExistedAsync(IList<string> personIds)
    {
        ICypherFluentQuery<List<string>> query = client.Cypher
            .WithParam("ids", personIds)
            .Match("(p:PERSON)")
            .Where("p.id IN $ids")
            .With("COLLECT(p.id) AS foundIds, $ids AS ids")
            .Return((foundIds, ids) => Return.As<List<string>>("[id IN ids WHERE NOT id IN foundIds]"));

        IEnumerable<List<string>>? idsNotExisted = await query.ResultsAsync;

        return idsNotExisted.FirstOrDefault();
    }
}