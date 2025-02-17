using Genealogy.Domain.Enums;

namespace Genealogy.Application.Helpers;

public static class RelationshipHelper
{
    public static Relationship GetCounterpartRelationship(Relationship relationship)
    {
        return relationship switch
        {
            Relationship.Wife => Relationship.Husband,
            Relationship.Husband => Relationship.Wife,
            _ => throw new IndexOutOfRangeException("Unknown relationship type")
        };
    }
}