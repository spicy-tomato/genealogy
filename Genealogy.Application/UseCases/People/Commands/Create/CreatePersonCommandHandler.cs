using Genealogy.Application.Models;
using Genealogy.Application.Services.Abstractions;
using Genealogy.Domain.Models;
using Genealogy.Infrastructure.Repositories.Abstractions;
using Genealogy.Infrastructure.Services.Abstractions;
using MediatR;

namespace Genealogy.Application.UseCases.People.Commands.Create;

public class CreatePersonCommandHandler(IFamilyService familyService, IPersonRepository personRepository,
    IRequestValidator<CreatePersonCommand> validator)
    : IRequestHandler<CreatePersonCommand, Response<string>>
{
    public async Task<Response<string>> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        await validator.Validate(request);

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