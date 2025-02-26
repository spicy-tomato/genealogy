using FluentValidation;
using Genealogy.Application.Models;
using Genealogy.Domain.Models;
using Genealogy.Infrastructure.Neo4j.Repositories.Abstractions;
using Genealogy.Infrastructure.Neo4j.Services.Abstractions;
using MediatR;

namespace Genealogy.Application.UseCases.People.Commands.Create;

public class CreatePersonCommandHandler(IFamilyService familyService, IPersonRepository personRepository,
    IValidator<CreatePersonCommand> validator)
    : IRequestHandler<CreatePersonCommand, Response<string>>
{
    public async Task<Response<string>> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        Person person = Person.Create(request.Name, request.BirthDate);
        string createdUserId;
        if (request.Relationship == null)
        {
            createdUserId = await personRepository.CreateAsync(person);
        }
        else
        {
            createdUserId = await familyService.AddPerson(person, request.Relationship.Value,
                request.AnotherPersonIds!);
        }

        return Response.Succeed(createdUserId);
    }
}