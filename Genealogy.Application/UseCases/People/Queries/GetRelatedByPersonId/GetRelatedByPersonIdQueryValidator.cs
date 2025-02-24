using FluentValidation;
using Genealogy.Application.Extensions;
using JetBrains.Annotations;

namespace Genealogy.Application.UseCases.People.Queries.GetRelatedByPersonId;

[UsedImplicitly]
public class GetRelatedByPersonIdQueryValidator : AbstractValidator<GetRelatedByPersonIdQuery>
{
    public GetRelatedByPersonIdQueryValidator()
    {
        RuleFor(q => q.RootId)
            .IsGuid();

        RuleFor(q => q.Depth)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(20);
    }
}