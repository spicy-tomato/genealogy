using Genealogy.Application.Helpers;
using Genealogy.Application.Models;
using Genealogy.Application.Services.Abstractions;
using Genealogy.Domain.Enums;
using Genealogy.Infrastructure.Repositories.Abstractions;
using MediatR;

namespace Genealogy.Application.UseCases.People.Connect;

public class ConnectPeopleCommandHandler(IPersonRepository personRepository,
    IRequestValidator<ConnectPeopleCommand> validator)
    : IRequestHandler<ConnectPeopleCommand, Response<KeyValuePair<string, string>>>
{
    public async Task<Response<KeyValuePair<string, string>>> Handle(ConnectPeopleCommand request,
        CancellationToken cancellationToken)
    {
        await validator.Validate(request);

        Relationship counterpartRelationship = RelationshipHelper.GetCounterpartRelationship(request.Relationship);

        KeyValuePair<string, string>? result = await personRepository.Connect(request.PersonId1, request.Relationship,
            request.PersonId2, counterpartRelationship);

        return result != null
            ? Response.Succeed(result.Value)
            : Response.Error<KeyValuePair<string, string>>("Cannot connect people");
    }
}