using Genealogy.Application.Models;
using Genealogy.Domain.Models;
using MediatR;

namespace Genealogy.Application.UseCases.People.Create;

public record CreatePersonCommand(
    string Name,
    string BirthDate,
    Dictionary<string, PersonRelationshipDetails> Relationships)
    : IRequest<Response<string>>;