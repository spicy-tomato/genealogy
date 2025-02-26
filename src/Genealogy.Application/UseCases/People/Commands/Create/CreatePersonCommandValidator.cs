using FluentValidation;
using Genealogy.Application.Extensions;
using Genealogy.Domain.Neo4j.Enums;
using JetBrains.Annotations;

namespace Genealogy.Application.UseCases.People.Commands.Create;

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

        RuleFor(c => c.AnotherPersonIds)
            .Must(ids => ids != null && ids.Any() && ids.Count <= 2)
            .WithMessage("One or two person IDs must be provided")
            .When(c => c.Relationship != null);

        RuleFor(c => c.AnotherPersonIds)
            .Must(ids => ids is { Count: 1 })
            .WithMessage(
                $"One person ID must be provided when relationship is {nameof(Relationship.Spouse)} or {nameof(Relationship.DivorceSpouse)}")
            .When(c => c.Relationship is Relationship.Spouse or Relationship.DivorceSpouse);

        RuleForEach(c => c.AnotherPersonIds)
            .IsGuid();
    }
}