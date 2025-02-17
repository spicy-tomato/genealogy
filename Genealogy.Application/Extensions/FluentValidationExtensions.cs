using FluentValidation;

namespace Genealogy.Application.Extensions;

public static class FluentValidationExtensions
{
    public static void IsGuid<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        ruleBuilder.Must(id => Guid.TryParse(id, out _))
            .WithMessage("{PropertyName} must be a valid GUID");
    }
}