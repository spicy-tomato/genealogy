using Genealogy.Application.Models;
using Genealogy.Application.Services.Abstractions;
using Genealogy.Domain.Models;
using Genealogy.Infrastructure.Repositories.Abstractions;
using MediatR;

namespace Genealogy.Application.UseCases.People.Create;

public class CreatePersonCommandHandler(IPersonRepository personRepository,
    IRequestValidator<CreatePersonCommand> validator)
    : IRequestHandler<CreatePersonCommand, Response<string>>
{
    public async Task<Response<string>> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        await validator.Validate(request);

        Person person = Person.Create(request.Name, request.BirthDate);
        string? createdUserId = await personRepository.Add(person);

        return createdUserId != null
            ? Response.Succeed(createdUserId)
            : Response.Error<string>("Cannot create person");
    }
}