using Genealogy.Domain.Enums;
using JetBrains.Annotations;

namespace Genealogy.Application.UseCases.People.Connect;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class ConnectPeopleRequest
{
    public string From { get; set; } = null!;
    public string To { get; set; } = null!;
    public Relationship Relationship { get; set; }
}