using JetBrains.Annotations;

namespace Genealogy.Application.UseCases.People.Create;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class CreatePersonRequest
{
    public string Name { get; set; } = null!;
    public string BirthDate { get; set; } = null!;
}