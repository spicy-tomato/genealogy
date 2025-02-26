using Genealogy.Domain.Neo4j.Enums;
using JetBrains.Annotations;

namespace Genealogy.Application.UseCases.People.Commands.Create;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class CreatePersonRequest
{
    public required string Name { get; set; }
    public required string BirthDate { get; set; }
    public Relationship? Relationship { get; set; }
    public IList<string>? AnotherPersonIds { get; set; }
}