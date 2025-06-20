﻿namespace Genealogy.Domain.Neo4j.Models;

public class Family
{
    public Family()
    {
    }

    private Family(bool? isDivorced)
    {
        Id = Guid.NewGuid().ToString();
        IsDivorced = isDivorced;
    }

    public string Id { get; set; } = null!;
    public bool? IsDivorced { get; set; }

    public static Family Create()
    {
        return new Family(null);
    }

    public static Family Create(bool isDivorced)
    {
        return new Family(isDivorced);
    }
}