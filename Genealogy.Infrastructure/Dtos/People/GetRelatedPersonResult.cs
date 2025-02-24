﻿using JetBrains.Annotations;

namespace Genealogy.Infrastructure.Dtos.People;

public class GetRelatedPersonResult
{
    public IEnumerable<GetRelatedPersonResultNode> Nodes { get; init; } = [];
    public IEnumerable<GetRelatedPersonResultLink> Links { get; init; } = [];
}

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
public class GetRelatedPersonResultNode
{
    public string Id { get; set; } = null!;
    public bool IsRoot { get; set; }
    public string Name { get; set; } = null!;
    public DateTime BirthDate { get; set; }
}

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
public class GetRelatedPersonResultLink
{
    public string Source { get; set; } = null!;
    public string Target { get; set; } = null!;
    public string Type { get; set; } = null!;
}