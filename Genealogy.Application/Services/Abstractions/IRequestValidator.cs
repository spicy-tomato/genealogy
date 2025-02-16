using MediatR;

namespace Genealogy.Application.Services.Abstractions;

public interface IRequestValidator<in T> where T : IBaseRequest
{
    Task Validate(T request);
}