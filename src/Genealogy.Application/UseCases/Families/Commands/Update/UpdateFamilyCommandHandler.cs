using FluentValidation;
using Genealogy.Application.Models;
using Genealogy.Infrastructure.Neo4j.Repositories.Abstractions;
using MediatR;

namespace Genealogy.Application.UseCases.Families.Commands.Update;

public class UpdateFamilyCommandHandler(IFamilyRepository familyRepository, IValidator<UpdateFamilyCommand> validator)
    : IRequestHandler<UpdateFamilyCommand, Response<bool>>
{
    public async Task<Response<bool>> Handle(UpdateFamilyCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        await familyRepository.Update(request.PersonId1, request.PersonId2, request.UpdateFamilyDto);

        return Response.Succeed(true);
    }
}