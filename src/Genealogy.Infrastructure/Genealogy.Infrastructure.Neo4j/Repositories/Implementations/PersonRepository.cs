using Genealogy.Domain.Neo4j.Models;
using Genealogy.Infrastructure.Exceptions;
using Genealogy.Infrastructure.Neo4j.Dtos.People;
using Genealogy.Infrastructure.Neo4j.Repositories.Abstractions;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace Genealogy.Infrastructure.Neo4j.Repositories.Implementations;

internal class PersonRepository(BoltGraphClient client) : IPersonRepository
{
    public async Task<Person?> GetByIdAsync(string id)
    {
        ICypherFluentQuery<Person> query = client.Cypher
            .Match("(p:Person)")
            .Where<Person>(p => p.Id == id)
            .Return<Person>("p");

        IEnumerable<Person>? queryResult = await query.ResultsAsync;

        return queryResult.FirstOrDefault();
    }

    public async Task<Person?> GetRootPersonIdAsync()
    {
        ICypherFluentQuery<Person> query = client.Cypher
            .Match("(p:Person)")
            .Where<Person>(p => p.IsRoot == true)
            .Return<Person>("p");

        IEnumerable<Person>? queryResult = await query.ResultsAsync;

        return queryResult.FirstOrDefault();
    }

    public async Task<GetRelatedPersonResult> GetRelatedByIdAsync(string id, int depth)
    {
        // Compute the maximum path length as 2 * levels.
        int maxLength = depth * 2;

        var query = client.Cypher
            .Match($"path = (p:Person {{id: $id}})-[:Parent|Child*2..{maxLength}]-(other:Person)")
            .WithParams(new { id })
            .With("collect(path) AS paths")
            // Unwind nodes from paths
            .Unwind("paths", "pth")
            .Unwind("nodes(pth)", "n")
            .With("collect(DISTINCT n) AS allNodes, paths")
            // Filter out only Person nodes
            .With("[node IN allNodes WHERE 'Person' IN labels(node)] AS personNodes, paths")
            // Unwind relationships for links
            .Unwind("paths", "pth2")
            .Unwind("relationships(pth2)", "r")
            .With("personNodes, collect(DISTINCT r) AS rels")
            .Return((personNodes, rels) => new
            {
                nodes = Return.As<IEnumerable<GetRelatedPersonResultNode>>("[node IN personNodes | node { .* }]"),
                links = Return.As<IEnumerable<GetRelatedPersonResultLink>>(
                    "[rel IN rels | { source: startNode(rel).id, target: endNode(rel).id, type: type(rel) }]")
            });

        var queryResult = (await query.ResultsAsync).FirstOrDefault();

        if (queryResult == null)
        {
            return new GetRelatedPersonResult();
        }

        GetRelatedPersonResult result = new()
        {
            Nodes = queryResult.nodes,
            Links = queryResult.links
        };

        return result;
    }

    public async Task<string> CreateAsync(Person person)
    {
        bool somePeopleExisted = await IsSomePeopleExisted();
        if (!somePeopleExisted)
        {
            person.MarkAsRoot();
        }

        ICypherFluentQuery query = client.Cypher
            .Create("(p:Person $person)")
            .WithParam("person", person);

        await query.ExecuteWithoutResultsAsync();

        return person.Id;
    }

    public async Task UpdateAsync(string id, UpdatePersonDto updatePersonDto)
    {
        Person? existingPerson = await GetByIdAsync(id);
        if (existingPerson == null)
        {
            throw NotFoundException.WithId<Person>(id);
        }

        ICypherFluentQuery query = client.Cypher
            .Match("(person:Family)")
            .Where<Family>(person => person.Id == existingPerson.Id)
            .Set("person.name=$name, person.birthDate=$birthDate")
            .WithParams(new Dictionary<string, object>
            {
                { "name", updatePersonDto.Name },
                { "birthDate", updatePersonDto.BirthDate }
            });

        await query.ExecuteWithoutResultsAsync();
    }

    public async Task DeleteAsync(string id)
    {
        ICypherFluentQuery query = client.Cypher
            .Match("(person:Person)")
            .Where<Person>(p => p.Id == id)
            .DetachDelete("person");

        await query.ExecuteWithoutResultsAsync();
    }

    public async Task<bool> IsSomePeopleExisted()
    {
        ICypherFluentQuery<Person> somePeopleExistedQuery = client.Cypher
            .OptionalMatch("(p:Person)")
            .Return<Person>("p")
            .Limit(1);

        IEnumerable<Person> result = await somePeopleExistedQuery.ResultsAsync;
        return result.FirstOrDefault() != null;
    }

    public async Task<List<string>?> GetIdsNotExistedAsync(IList<string> personIds)
    {
        ICypherFluentQuery<List<string>> query = client.Cypher
            .WithParam("ids", personIds)
            .Match("(p:Person)")
            .Where("p.id IN $ids")
            .With("COLLECT(p.id) AS foundIds, $ids AS ids")
            .Return((foundIds, ids) => Return.As<List<string>>("[id IN ids WHERE NOT id IN foundIds]"));

        IEnumerable<List<string>>? idsNotExisted = await query.ResultsAsync;

        return idsNotExisted.FirstOrDefault();
    }

    public async Task UnmarkAsRootAsync(string id)
    {
        Person? existingPerson = await GetByIdAsync(id);
        if (existingPerson == null)
        {
            throw NotFoundException.WithId<Person>(id);
        }

        if (!existingPerson.IsRoot)
        {
            throw BadRequestException.Create("Person is not a root person");
        }

        ICypherFluentQuery query = client.Cypher
            .Match("(person:Person)")
            .Where<Person>(person => person.Id == id)
            .Set("person.isRoot=false");

        await query.ExecuteWithoutResultsAsync();
    }
}