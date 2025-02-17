using Genealogy.Application.Models;
using Genealogy.Domain.Enums;
using MediatR;

namespace Genealogy.Application.UseCases.People.Connect;

public record ConnectPeopleCommand(string PersonId1, Relationship Relationship, string PersonId2)
    : IRequest<Response<KeyValuePair<string, string>>>;