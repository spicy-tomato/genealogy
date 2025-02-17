using Genealogy.Domain.Enums;

namespace Genealogy.Domain.Models;

public class PersonRelationshipDetails(Relationship relationship, Relationship reversedRelationship)
{
    public Relationship Relationship { get; } = relationship;
    public Relationship ReversedRelationship { get; } = reversedRelationship;

    public PersonRelationshipDetails Reversed()
    {
        return new PersonRelationshipDetails(ReversedRelationship, Relationship);
    }

    private bool Equals(PersonRelationshipDetails other)
    {
        return Relationship == other.Relationship && ReversedRelationship == other.ReversedRelationship;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((PersonRelationshipDetails)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)Relationship, (int)ReversedRelationship);
    }
}