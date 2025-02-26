using Genealogy.Domain.Neo4j.Models;
using Genealogy.Infrastructure.Neo4j.Dtos.People;

namespace Genealogy.Infrastructure.Neo4j.Repositories.Abstractions;

public interface IPersonRepository : IRepository
{
    Task<Person?> GetByIdAsync(string id);
    Task<Person?> GetRootPersonIdAsync();
    Task<GetRelatedPersonResult> GetRelatedByIdAsync(string id, int depth);
    Task<string> CreateAsync(Person person);
    Task UpdateAsync(string id, UpdatePersonDto updatePersonDto);
    Task DeleteAsync(string id);
    Task<bool> IsSomePeopleExisted();
    Task<List<string>?> GetIdsNotExistedAsync(IList<string> personIds);
    Task UnmarkAsRootAsync(string id);
}