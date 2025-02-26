using JetBrains.Annotations;

namespace Genealogy.Infrastructure.Neo4j.Dtos.Families;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class UpdateFamilyDto
{
    public bool IsDivorced { get; set; }
}