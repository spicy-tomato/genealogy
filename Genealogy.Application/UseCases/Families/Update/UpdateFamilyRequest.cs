using Genealogy.Infrastructure.Dtos.Family;
using JetBrains.Annotations;

namespace Genealogy.Application.UseCases.Families.Update;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class UpdateFamilyRequest
{
    public required string Person1 { get; set; }
    public required string Person2 { get; set; }
    public required UpdateFamilyDto Family { get; set; }
}