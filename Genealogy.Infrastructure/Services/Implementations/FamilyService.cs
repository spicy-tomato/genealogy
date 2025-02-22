using Genealogy.Domain.Models;
using Genealogy.Infrastructure.Exceptions;
using Genealogy.Infrastructure.Repositories.Abstractions;
using Genealogy.Infrastructure.Repositories.Implementations;
using Genealogy.Infrastructure.Services.Abstractions;
using Microsoft.Extensions.Logging;
using Neo4jClient;
using Neo4jClient.Transactions;
using Relationship = Genealogy.Domain.Enums.Relationship;

namespace Genealogy.Infrastructure.Services.Implementations;

internal class FamilyService(BoltGraphClient client, IFamilyRepository familyRepository,
    IPersonRepository personRepository,
    ILogger<PersonRepository> logger) : IFamilyService
{
    public async Task<string> AddPerson(Person person, Relationship relationship, IList<string> anotherPersonIds)
    {
        using ITransaction transaction = client.BeginTransaction();

        try
        {
            await personRepository.CreateAsync(person);
            await Connect(person.Id, relationship, anotherPersonIds);

            await transaction.CommitAsync();

            return person.Id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error: {Message}", ex.Message);

            await transaction.RollbackAsync();

            throw;
        }
    }

    private async Task Connect(string personId, Relationship relationship, IList<string> anotherPersonIds)
    {
        List<string>? idsNotExisted = await personRepository.GetIdsNotExistedAsync(anotherPersonIds);
        if (idsNotExisted != null && idsNotExisted.Count != 0)
        {
            throw NotFoundException.WithId<Person>(idsNotExisted);
        }

        switch (relationship)
        {
            case Relationship.Spouse:
                await ConnectAsSpouse(personId, anotherPersonIds.First());
                break;
            case Relationship.DivorceSpouse:
                await ConnectAsSpouse(personId, anotherPersonIds.First(), true);
                break;
            case Relationship.Child:
                await ConnectAsChild(personId, anotherPersonIds);
                break;
            case Relationship.AdoptedChild:
                await ConnectAsChild(personId, anotherPersonIds, true);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(relationship));
        }
    }

    private async Task ConnectAsSpouse(string personId, string spouseId, bool divorced = false)
    {
        await familyRepository.CreateAsync(personId, spouseId, divorced);
    }

    private async Task ConnectAsChild(string personId, IList<string> parentIds, bool isAdopted = false)
    {
        if (parentIds.Count == 1)
        {
            await ConnectAsChild(personId, parentIds.First(), isAdopted);
        }
        else
        {
            await ConnectAsChild(personId, parentIds[0], parentIds[1], isAdopted);
        }
    }

    private async Task ConnectAsChild(string personId, string parentId, bool isAdopted)
    {
        Family? singleFamily = await familyRepository.GetSingleFamilyAsync(parentId);

        if (singleFamily != null)
        {
            await familyRepository.AddPersonAsync(singleFamily.Id, personId, isAdopted);

            return;
        }

        string newSingleFamilyId = await familyRepository.CreateAsync(parentId);

        await familyRepository.AddPersonAsync(newSingleFamilyId, personId, isAdopted);
    }

    private async Task ConnectAsChild(string personId, string parentId1, string parentId2, bool isAdopted)
    {
        Family? family = await familyRepository.GetFamilyAsync(parentId1, parentId2);
        if (family == null)
        {
            throw NotFoundException.Create($"{nameof(Family)} relationship is not found.");
        }

        await familyRepository.AddPersonAsync(family.Id, personId, isAdopted);
    }
}