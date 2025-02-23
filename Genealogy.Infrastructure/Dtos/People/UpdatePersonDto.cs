using JetBrains.Annotations;

namespace Genealogy.Infrastructure.Dtos.People;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class UpdatePersonDto
{
    public string Name { get; set; } = null!;
    public DateTime BirthDate { get; set; }
}