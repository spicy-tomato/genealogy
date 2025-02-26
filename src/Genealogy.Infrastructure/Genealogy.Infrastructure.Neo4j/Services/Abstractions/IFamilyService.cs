using Genealogy.Domain.Neo4j.Enums;
using Genealogy.Domain.Neo4j.Models;

namespace Genealogy.Infrastructure.Neo4j.Services.Abstractions;

public interface IFamilyService
{
    Task<string> AddPerson(Person person, Relationship relationship, IList<string> anotherPersonIds);
}