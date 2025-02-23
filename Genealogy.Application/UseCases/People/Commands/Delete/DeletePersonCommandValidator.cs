using FluentValidation;
using Genealogy.Application.Extensions;
using JetBrains.Annotations;

namespace Genealogy.Application.UseCases.People.Commands.Delete;

[UsedImplicitly(ImplicitUseKindFlags.Access)]
public class DeletePersonCommandValidator : AbstractValidator<DeletePersonCommand>
{
    public DeletePersonCommandValidator()
    {
        RuleFor(c => c.Id)
            .IsGuid();
    }
}