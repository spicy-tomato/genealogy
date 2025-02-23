using JetBrains.Annotations;

namespace Genealogy.Infrastructure.Dtos.Families;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class UpdateFamilyDto
{
    public bool IsDivorced { get; set; }
}