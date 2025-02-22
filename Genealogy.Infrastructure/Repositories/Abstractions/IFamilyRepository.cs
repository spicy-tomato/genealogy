using Genealogy.Domain.Models;

namespace Genealogy.Infrastructure.Repositories.Abstractions;

public interface IFamilyRepository : IRepository
{
    Task<Family?> GetSingleFamilyAsync(string parentId);
    Task<Family?> GetFamilyAsync(string parentId1, string parentId2);
    Task<string> CreateAsync(string parentId);
    Task<string> CreateAsync(string personId, string anotherId, bool isDivorced);
    Task AddPersonAsync(string familyId, string personId, bool isAdopted);
}