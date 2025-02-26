using FluentValidation;

namespace Genealogy.Application.Extensions;

public static class FluentValidationExtensions
{
    public static void IsGuid<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        ruleBuilder.Must(value => value == null || Guid.TryParse(value, out _))
            .WithMessage("{PropertyName} must be a valid GUID");
    }
}