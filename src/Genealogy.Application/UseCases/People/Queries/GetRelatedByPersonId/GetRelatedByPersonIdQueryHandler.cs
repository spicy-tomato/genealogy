using FluentValidation;
using Genealogy.Application.Models;
using Genealogy.Domain.Models;
using Genealogy.Infrastructure.Dtos.People;
using Genealogy.Infrastructure.Exceptions;
using Genealogy.Infrastructure.Repositories.Abstractions;
using MediatR;

namespace Genealogy.Application.UseCases.People.Queries.GetRelatedByPersonId;

public class GetRelatedByPersonIdQueryHandler(IPersonRepository personRepository,
    IValidator<GetRelatedByPersonIdQuery> validator)
    : IRequestHandler<GetRelatedByPersonIdQuery, Response<GetRelatedPersonResult>>
{
    public async Task<Response<GetRelatedPersonResult>> Handle(GetRelatedByPersonIdQuery request,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        if (!await personRepository.IsSomePeopleExisted())
        {
            return Response.Succeed(new GetRelatedPersonResult());
        }

        string? rootId = request.RootId;

        if (string.IsNullOrEmpty(rootId))
        {
            Person? rootPerson = await personRepository.GetRootPersonIdAsync();
            if (rootPerson == null)
            {
                throw InternalServerException.Create("No root person found");
            }

            rootId = rootPerson.Id;
        }
        else
        {
            if (await personRepository.GetByIdAsync(rootId) == null)
            {
                throw NotFoundException.WithId<Person>(rootId);
            }
        }

        GetRelatedPersonResult result = await personRepository.GetRelatedByIdAsync(rootId, request.Depth);

        return Response.Succeed(result);
    }
}