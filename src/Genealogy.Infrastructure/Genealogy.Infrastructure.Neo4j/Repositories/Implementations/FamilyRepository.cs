using Genealogy.Domain.Models;
using Genealogy.Infrastructure.Exceptions;
using Genealogy.Infrastructure.Neo4j.Dtos.Families;
using Genealogy.Infrastructure.Neo4j.Repositories.Abstractions;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace Genealogy.Infrastructure.Neo4j.Repositories.Implementations;

internal class FamilyRepository(BoltGraphClient client) : IFamilyRepository
{
    public async Task<Family?> GetSingleByParentsIdAsync(string parentId)
    {
        ICypherFluentQuery<Family> query = client.Cypher
            .OptionalMatch("(person:Person)-[r:Parent]->(family:Family)")
            .Where<Person>(person => person.Id == parentId)
            .With("family")
            .OptionalMatch("(:Person)-[r:Parent]->(family)")
            .With("family, COUNT(r) as cnt")
            .Where("cnt = 1")
            .Return<Family>("family");

        IEnumerable<Family?> singleFamilyResult = (await query.ResultsAsync).ToList();

        return singleFamilyResult.FirstOrDefault();
    }

    public async Task<Family?> GetByParentsIdAsync(string parentId1, string parentId2)
    {
        ICypherFluentQuery<Family>? query = client.Cypher
            .OptionalMatch("(p1:Person)-[:Parent]->(family:Family)<-[:Parent]-(p2:Person)")
            .Where<Person, Person>((p1, p2) => p1.Id == parentId1 && p2.Id == parentId2)
            .Return(family => family.As<Family>());

        List<Family> singleFamilyResult = (await query.ResultsAsync).ToList();

        return singleFamilyResult.FirstOrDefault();
    }

    public async Task<string> CreateAsync(string parentId)
    {
        Family? existingSingleFamily = await GetSingleByParentsIdAsync(parentId);
        if (existingSingleFamily != null)
        {
            return existingSingleFamily.Id;
        }

        Family family = Family.Create();

        ICypherFluentQuery query = client.Cypher
            .Match("(person:Person)")
            .Where<Person>(person => person.Id == parentId)
            .Create("(person)-[:Parent]->(family:Family $family)")
            .WithParam("family", family);

        await query.ExecuteWithoutResultsAsync();

        return family.Id;
    }

    public async Task<string> CreateAsync(string personId, string anotherId, bool isDivorced)
    {
        Family family = Family.Create(isDivorced);

        ICypherFluentQuery query = client.Cypher
            .Match("(person:Person)", "(another:Person)")
            .Where<Person>(person => person.Id == personId)
            .AndWhere<Person>(another => another.Id == anotherId)
            .Merge("(person)-[:Parent]->(family:Family)<-[:Parent]-(another)")
            .OnCreate()
            .Set("family=$family")
            .WithParam("family", family);

        await query.ExecuteWithoutResultsAsync();

        return family.Id;
    }

    public async Task AddPersonAsync(string familyId, string personId, bool isAdopted)
    {
        string child = isAdopted ? "AdoptedChild" : "Child";

        ICypherFluentQuery query = client.Cypher
            .Match("(family:Family)", "(person:Person)")
            .Where<Family>(family => family.Id == familyId)
            .AndWhere<Person>(person => person.Id == personId)
            .Merge($"(person)-[:{child}]->(family)");

        await query.ExecuteWithoutResultsAsync();
    }

    public async Task Update(string personId1, string personId2, UpdateFamilyDto updateFamilyDto)
    {
        Family? existingFamily = await GetByParentsIdAsync(personId1, personId2);
        if (existingFamily == null)
        {
            throw NotFoundException.Create($"{nameof(Family)} relationship is not found.");
        }

        ICypherFluentQuery query = client.Cypher
            .Match("(family:Family)")
            .Where<Family>(family => family.Id == existingFamily.Id)
            .Set("family.isDivorced=$isDivorced")
            .WithParam("isDivorced", updateFamilyDto.IsDivorced);

        await query.ExecuteWithoutResultsAsync();
    }
}