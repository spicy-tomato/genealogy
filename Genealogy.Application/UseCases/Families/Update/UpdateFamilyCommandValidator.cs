using FluentValidation;
using Genealogy.Application.Extensions;
using JetBrains.Annotations;

namespace Genealogy.Application.UseCases.Families.Update;

[UsedImplicitly(ImplicitUseKindFlags.Access)]
public class UpdateFamilyCommandValidator : AbstractValidator<UpdateFamilyCommand>
{
    public UpdateFamilyCommandValidator()
    {
        RuleFor(c => c.PersonId1)
            .IsGuid();

        RuleFor(c => c.PersonId2)
            .IsGuid();

        RuleFor(c => c.UpdateFamilyDto)
            .NotNull();
    }
}