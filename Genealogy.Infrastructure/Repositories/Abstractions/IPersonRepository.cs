using Genealogy.Domain.Enums;
using Genealogy.Domain.Models;

namespace Genealogy.Infrastructure.Repositories.Abstractions;

public interface IPersonRepository : IRepository
{
    Task<string?> Add(Person person);

    Task<KeyValuePair<string, string>?> Connect(string personId1, Relationship relationship, string personId2,
        Relationship reversedRelationship);

    Task<bool> Delete(string id);
}