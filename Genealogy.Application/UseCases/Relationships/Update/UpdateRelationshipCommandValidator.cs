using FluentValidation;
using Genealogy.Application.Extensions;
using JetBrains.Annotations;

namespace Genealogy.Application.UseCases.Relationships.Update;

[UsedImplicitly(ImplicitUseKindFlags.Access)]
public class UpdateRelationshipCommandValidator : AbstractValidator<UpdateRelationshipCommand>
{
    public UpdateRelationshipCommandValidator()
    {
        RuleFor(c => c.PersonId1)
            .IsGuid();

        RuleFor(c => c.PersonId2)
            .IsGuid();

        RuleFor(c => c.ChangeType)
            .IsInEnum();
    }
}