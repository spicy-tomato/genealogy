using FluentValidation;
using JetBrains.Annotations;

namespace Genealogy.Application.UseCases.People.Create;

[UsedImplicitly(ImplicitUseKindFlags.Access)]
public class CreatePersonCommandValidator : AbstractValidator<CreatePersonCommand>
{
    public CreatePersonCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .NotNull()
            .MaximumLength(100);

        RuleFor(c => c.BirthDate)
            .NotEmpty()
            .NotNull()
            .Must(bd => DateTime.TryParse(bd, out _))
            .WithMessage("Invalid Birth Date");
    }
}