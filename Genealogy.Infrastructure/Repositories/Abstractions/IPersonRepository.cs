using Genealogy.Domain.Enums;
using Genealogy.Domain.Models;

namespace Genealogy.Infrastructure.Repositories.Abstractions;

public interface IPersonRepository : IRepository
{
    Task<string?> Add(Person person);

    Task<KeyValuePair<string, string>?> Connect(string personId1, Relationship relationship1, string personId2,
        Relationship relationship2);
}