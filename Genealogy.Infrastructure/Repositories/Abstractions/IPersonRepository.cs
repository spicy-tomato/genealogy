using Genealogy.Domain.Models;
using Genealogy.Infrastructure.Dtos.People;

namespace Genealogy.Infrastructure.Repositories.Abstractions;

public interface IPersonRepository : IRepository
{
    Task<Person?> GetByIdAsync(string id);
    Task<string> CreateAsync(Person person);
    Task UpdateAsync(string id, UpdatePersonDto updatePersonDto);
    Task DeleteAsync(string id);
    Task<List<string>?> GetIdsNotExistedAsync(IList<string> personIds);
    Task UnmarkAsRootAsync(string id);
}