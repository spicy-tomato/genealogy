using Genealogy.Application.Models;
using Genealogy.Application.Services.Abstractions;
using Genealogy.Domain.Models;
using Genealogy.Infrastructure.Repositories.Abstractions;
using MediatR;

namespace Genealogy.Application.UseCases.People.Create;

public class CreatePersonCommandHandler(IPersonRepository personRepository,
    IRequestValidator<CreatePersonCommand> validator)
    : IRequestHandler<CreatePersonCommand, Response<Guid>>
{
    public async Task<Response<Guid>> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        await validator.Validate(request);

        Person person = Person.Create(request.Name, request.BirthDate);
        Guid? id = await personRepository.Add(person);

        return id != null
            ? Response.Succeed(id.Value)
            : Response.Error<Guid>("Cannot create person");
    }
}