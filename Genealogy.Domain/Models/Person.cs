namespace Genealogy.Domain.Models;

public class Person
{
    private Person(string name, DateTime birthDate, Dictionary<string, PersonRelationshipDetails> relationships)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        BirthDate = birthDate;
        Relationships = relationships;
    }

    public string Id { get; }
    public string Name { get; }
    public DateTime BirthDate { get; }
    public Dictionary<string, PersonRelationshipDetails> Relationships { get; set; } = new();

    public static Person Create(string name, string birthDate,
        Dictionary<string, PersonRelationshipDetails> relationships)
    {
        return new Person(name, DateTime.Parse(birthDate), relationships);
    }
}