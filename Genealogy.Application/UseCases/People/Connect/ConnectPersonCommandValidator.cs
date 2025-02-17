using FluentValidation;
using JetBrains.Annotations;

namespace Genealogy.Application.UseCases.People.Connect;

[UsedImplicitly(ImplicitUseKindFlags.Access)]
public class ConnectPeopleCommandValidator : AbstractValidator<ConnectPeopleCommand>
{
    public ConnectPeopleCommandValidator()
    {
        RuleFor(c => c.PersonId1)
            .NotEmpty()
            .NotNull();

        RuleFor(c => c.PersonId2)
            .NotEmpty()
            .NotNull();

        RuleFor(c => c.Relationship)
            .IsInEnum();
    }
}