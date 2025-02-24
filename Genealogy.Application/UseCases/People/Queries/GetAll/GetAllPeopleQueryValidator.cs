using FluentValidation;
using Genealogy.Application.Extensions;

namespace Genealogy.Application.UseCases.People.Queries.GetAll;

public class GetAllPeopleQueryValidator : AbstractValidator<GetAllPeopleQuery>
{
    public GetAllPeopleQueryValidator()
    {
        RuleFor(q => q.RootId)
            .IsGuid();

        RuleFor(q => q.Depth)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(20);
    }
}