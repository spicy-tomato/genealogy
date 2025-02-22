using Genealogy.Domain.Enums;
using JetBrains.Annotations;

namespace Genealogy.Application.UseCases.Relationships.Update;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class UpdateRelationshipRequest
{
    public string Person1 { get; set; } = null!;
    public string Person2 { get; set; } = null!;
    public RelationshipChangeType ChangeType { get; set; }
}