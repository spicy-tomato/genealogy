using Genealogy.Domain.Enums;
using JetBrains.Annotations;

namespace Genealogy.Application.UseCases.People.Commands.Create;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class CreatePersonRequest
{
    public string Name { get; set; } = null!;
    public string BirthDate { get; set; } = null!;
    public Relationship? Relationship { get; set; }
    public IList<string>? AnotherPersonIds { get; set; }
}