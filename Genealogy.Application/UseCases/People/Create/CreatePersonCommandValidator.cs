using FluentValidation;
using Genealogy.Application.Extensions;
using Genealogy.Domain.Enums;
using Genealogy.Domain.Models;
using JetBrains.Annotations;

namespace Genealogy.Application.UseCases.People.Create;

[UsedImplicitly(ImplicitUseKindFlags.Access)]
public class CreatePersonCommandValidator : AbstractValidator<CreatePersonCommand>
{
    private readonly List<PersonRelationshipDetails> _validRelationshipPair =
    [
        new(Relationship.Husband, Relationship.Wife)
    ];

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

        RuleForEach(c => c.Relationships).ChildRules(relationship =>
        {
            relationship.RuleFor(r => r.Key)
                .IsGuid();
            relationship.RuleFor(r => r.Value)
                .Must(x => _validRelationshipPair.Exists(d => d.Equals(x)) ||
                    _validRelationshipPair.Exists(d => d.Equals(x.Reversed())))
                .WithMessage("Invalid Relationship");
        });
    }
}