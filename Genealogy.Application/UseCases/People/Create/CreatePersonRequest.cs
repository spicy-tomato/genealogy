using Genealogy.Domain.Models;
using JetBrains.Annotations;

namespace Genealogy.Application.UseCases.People.Create;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class CreatePersonRequest
{
    public string Name { get; set; } = null!;
    public string BirthDate { get; set; } = null!;
    public Dictionary<string, PersonRelationshipDetails> Relationships { get; set; } = null!;
}