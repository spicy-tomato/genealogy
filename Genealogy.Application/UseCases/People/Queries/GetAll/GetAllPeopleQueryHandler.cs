using FluentValidation;
using Genealogy.Application.Models;
using Genealogy.Domain.Models;
using Genealogy.Infrastructure.Repositories.Abstractions;
using MediatR;

namespace Genealogy.Application.UseCases.People.Queries.GetAll;

public class GetAllPeopleQueryHandler(IPersonRepository personRepository, IFamilyRepository familyRepository,
    IValidator<GetAllPeopleQuery> validator)
    : IRequestHandler<GetAllPeopleQuery, Response<IEnumerable<Person>>>
{
    public async Task<Response<IEnumerable<Person>>> Handle(GetAllPeopleQuery request,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        return await Task.FromResult(Response.Succeed(new List<Person>().AsEnumerable()));
    }
}