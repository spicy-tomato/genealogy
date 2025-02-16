using Genealogy.Application.Models;
using JetBrains.Annotations;
using MediatR;

namespace Genealogy.Application.UseCases.People.Create;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class CreatePersonRequest : IRequest<Response<Guid>>
{
    public string Name { get; set; } = null!;
    public string BirthDate { get; set; } = null!;
}