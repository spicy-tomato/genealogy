using Genealogy.Application.Models;
using Genealogy.Domain.Enums;
using MediatR;

namespace Genealogy.Application.UseCases.People.Create;

public record CreatePersonCommand(
    string Name,
    string BirthDate,
    Relationship? Relationship,
    IList<string>? AnotherPersonIds)
    : IRequest<Response<string>>;