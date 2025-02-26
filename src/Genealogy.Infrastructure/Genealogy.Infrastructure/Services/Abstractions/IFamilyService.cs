using Genealogy.Domain.Enums;
using Genealogy.Domain.Models;

namespace Genealogy.Infrastructure.Services.Abstractions;

public interface IFamilyService
{
    Task<string> AddPerson(Person person, Relationship relationship, IList<string> anotherPersonIds);
}