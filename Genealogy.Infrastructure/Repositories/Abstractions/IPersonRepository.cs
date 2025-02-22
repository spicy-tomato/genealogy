using Genealogy.Domain.Models;

namespace Genealogy.Infrastructure.Repositories.Abstractions;

public interface IPersonRepository : IRepository
{
    Task<string> CreateAsync(Person person);
    Task DeleteAsync(string id);
    Task<List<string>?> GetIdsNotExistedAsync(IList<string> personIds);
}