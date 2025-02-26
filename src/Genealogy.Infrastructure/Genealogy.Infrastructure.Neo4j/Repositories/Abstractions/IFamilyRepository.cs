using Genealogy.Domain.Neo4j.Models;
using Genealogy.Infrastructure.Neo4j.Dtos.Families;

namespace Genealogy.Infrastructure.Neo4j.Repositories.Abstractions;

public interface IFamilyRepository : IRepository
{
    Task<Family?> GetSingleByParentsIdAsync(string parentId);
    Task<Family?> GetByParentsIdAsync(string parentId1, string parentId2);
    Task<string> CreateAsync(string parentId);
    Task<string> CreateAsync(string personId, string anotherId, bool isDivorced);
    Task AddPersonAsync(string familyId, string personId, bool isAdopted);
    Task Update(string personId1, string personId2, UpdateFamilyDto updateFamilyDto);
}