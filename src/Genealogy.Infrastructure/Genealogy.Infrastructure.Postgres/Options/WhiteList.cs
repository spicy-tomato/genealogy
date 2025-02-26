using JetBrains.Annotations;

namespace Genealogy.Infrastructure.Postgres.Options;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class WhiteList
{
    public IEnumerable<string> Emails { get; set; } = [];
}