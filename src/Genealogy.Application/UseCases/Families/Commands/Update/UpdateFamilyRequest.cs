using Genealogy.Infrastructure.Dtos.Families;
using JetBrains.Annotations;

namespace Genealogy.Application.UseCases.Families.Commands.Update;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class UpdateFamilyRequest
{
    public required string Person1 { get; set; }
    public required string Person2 { get; set; }
    public required UpdateFamilyDto Family { get; set; }
}