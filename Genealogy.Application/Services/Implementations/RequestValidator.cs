using FluentValidation;
using FluentValidation.Results;
using Genealogy.Application.Services.Abstractions;
using MediatR;

namespace Genealogy.Application.Services.Implementations;

internal class RequestValidator<T>(IValidator<T> validator) : IRequestValidator<T> where T : IBaseRequest
{
    public async Task Validate(T request)
    {
        // checking validation
        ValidationResult? personValidator = await validator.ValidateAsync(request);
        if (!personValidator.IsValid)
        {
            throw new ValidationException(personValidator.Errors);
        }
    }
}