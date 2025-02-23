using JetBrains.Annotations;

namespace Genealogy.Infrastructure.Dtos.Family;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class UpdateFamilyDto
{
    public bool IsDivorced { get; set; }
}