using Genealogy.Domain.Models;

namespace Genealogy.Infrastructure.Repositories.Abstractions;

public interface IPersonRepository : IRepository
{
    Task<Guid?> Add(Person person);
}