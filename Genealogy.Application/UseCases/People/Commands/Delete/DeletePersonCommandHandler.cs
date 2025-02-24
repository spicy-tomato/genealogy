using FluentValidation;
using Genealogy.Application.Models;
using Genealogy.Infrastructure.Repositories.Abstractions;
using MediatR;

namespace Genealogy.Application.UseCases.People.Commands.Delete;

public class DeletePersonCommandHandler(IPersonRepository repository, IValidator<DeletePersonCommand> validator)
    : IRequestHandler<DeletePersonCommand, Response<bool>>
{
    public async Task<Response<bool>> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        await repository.DeleteAsync(request.Id);

        return Response.Succeed(true);
    }
}