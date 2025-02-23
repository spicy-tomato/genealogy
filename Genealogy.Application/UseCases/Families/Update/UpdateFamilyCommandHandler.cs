using Genealogy.Application.Models;
using Genealogy.Application.Services.Abstractions;
using Genealogy.Infrastructure.Repositories.Abstractions;
using MediatR;

namespace Genealogy.Application.UseCases.Families.Update;

public class UpdateFamilyCommandHandler(IFamilyRepository familyRepository,
    IRequestValidator<UpdateFamilyCommand> validator)
    : IRequestHandler<UpdateFamilyCommand, Response<bool>>
{
    public async Task<Response<bool>> Handle(UpdateFamilyCommand request, CancellationToken cancellationToken)
    {
        await validator.Validate(request);

        await familyRepository.Update(request.PersonId1, request.PersonId2, request.UpdateFamilyDto);

        return Response.Succeed(true);
    }
}