using Genealogy.Domain.Enums;
using Genealogy.Domain.Models;
using Genealogy.Infrastructure.Repositories.Abstractions;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace Genealogy.Infrastructure.Repositories.Implementations;

internal class FamilyRepository(BoltGraphClient client) : IFamilyRepository
{
    public async Task<Family?> GetSingleFamilyAsync(string parentId)
    {
        const string parent = nameof(FamilyRelationship.Parent);

        ICypherFluentQuery<Family> query = client.Cypher
            .OptionalMatch($"(person:Person)-[r:{parent}]->(family:Family)")
            .Where<Person>(person => person.Id == parentId)
            .With("family")
            .OptionalMatch($"(:Person)-[r:{parent}]->(family)")
            .With("family, COUNT(r) as cnt")
            .Where("cnt = 1")
            .Return<Family>("family");

        IEnumerable<Family?> singleFamilyResult = (await query.ResultsAsync).ToList();

        return singleFamilyResult.FirstOrDefault();
    }

    public async Task<Family?> GetFamilyAsync(string parentId1, string parentId2)
    {
        const string parent = nameof(FamilyRelationship.Parent);

        ICypherFluentQuery<Family>? query = client.Cypher
            .OptionalMatch($"(p1:Person)-[:{parent}]->(family:Family)<-[:{parent}]-(p2:Person)")
            .Where<Person, Person>((p1, p2) => p1.Id == parentId1 && p2.Id == parentId2)
            .Return(family => family.As<Family>());

        List<Family> singleFamilyResult = (await query.ResultsAsync).ToList();

        return singleFamilyResult.FirstOrDefault();
    }

    public async Task<string> CreateAsync(string parentId)
    {
        Family? existingSingleFamily = await GetSingleFamilyAsync(parentId);
        if (existingSingleFamily != null)
        {
            return existingSingleFamily.Id;
        }
            
        const string parent = nameof(FamilyRelationship.Parent);
        Family family = Family.Create();

        ICypherFluentQuery query = client.Cypher
            .Match("(person:Person)")
            .Where<Person>(person => person.Id == parentId)
            .Create($"(person)-[:{parent}]->(family:Family $family)")
            .WithParam("family", family);

        await query.ExecuteWithoutResultsAsync();

        return family.Id;
    }

    public async Task<string> CreateAsync(string personId, string anotherId, bool isDivorced)
    {
        const string parent = nameof(FamilyRelationship.Parent);
        Family family = Family.Create(isDivorced);

        ICypherFluentQuery query = client.Cypher
            .Match("(person:Person)", "(another:Person)")
            .Where<Person>(person => person.Id == personId)
            .AndWhere<Person>(another => another.Id == anotherId)
            .Merge($"(person)-[:{parent}]->(family:Family)<-[:{parent}]-(another)")
            .OnCreate()
            .Set("family=$family")
            .WithParam("family", family);

        await query.ExecuteWithoutResultsAsync();

        return family.Id;
    }

    public async Task AddPersonAsync(string familyId, string personId, bool isAdopted)
    {
        string child = isAdopted ? nameof(FamilyRelationship.AdoptedChild) : nameof(FamilyRelationship.Child);

        ICypherFluentQuery query = client.Cypher
            .Match("(family:Family)", "(person:Person)")
            .Where<Family>(family => family.Id == familyId)
            .AndWhere<Person>(person => person.Id == personId)
            .Merge($"(person)-[:{child}]->(family)");

        await query.ExecuteWithoutResultsAsync();
    }
}