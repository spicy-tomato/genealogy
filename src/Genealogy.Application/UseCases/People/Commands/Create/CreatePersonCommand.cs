using Genealogy.Application.Models;
using Genealogy.Domain.Neo4j.Enums;
using MediatR;

namespace Genealogy.Application.UseCases.People.Commands.Create;

public record CreatePersonCommand(
    string Name,
    string BirthDate,
    Relationship? Relationship,
    IList<string>? AnotherPersonIds)
    : IRequest<Response<string>>;