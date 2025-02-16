namespace Genealogy.Domain.Models;

public class Person
{
    private Person(string name, DateTime birthDate)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        BirthDate = birthDate;
    }

    public string Id { get; }
    public string Name { get; }
    public DateTime BirthDate { get; }

    public static Person Create(string name, string birthDate)
    {
        return new Person(name, DateTime.Parse(birthDate));
    }
}