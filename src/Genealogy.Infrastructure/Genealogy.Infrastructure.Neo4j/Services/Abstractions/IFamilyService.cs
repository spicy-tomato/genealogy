using Genealogy.Domain.Enums;
using Genealogy.Domain.Models;

namespace Genealogy.Infrastructure.Neo4j.Services.Abstractions;

public interface IFamilyService
{
    Task<string> AddPerson(Person person, Relationship relationship, IList<string> anotherPersonIds);
}