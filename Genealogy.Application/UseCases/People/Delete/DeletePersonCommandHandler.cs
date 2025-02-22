using Genealogy.Application.Models;
using Genealogy.Application.Services.Abstractions;
using Genealogy.Infrastructure.Repositories.Abstractions;
using MediatR;

namespace Genealogy.Application.UseCases.People.Delete;

public class DeletePersonCommandHandler(IPersonRepository repository, IRequestValidator<DeletePersonCommand> validator)
    : IRequestHandler<DeletePersonCommand, Response<bool>>
{
    public async Task<Response<bool>> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
    {
        await validator.Validate(request);

        await repository.DeleteAsync(request.Id);

        return Response.Succeed(true);
    }
}