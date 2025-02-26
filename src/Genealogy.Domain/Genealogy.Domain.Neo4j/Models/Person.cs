using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Genealogy.Domain.Neo4j.Models;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class Person
{
    public Person()
    {
    }

    private Person(string name, DateTime birthDate)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        BirthDate = birthDate;
    }

    public string Id { get; set; } = null!;
    public bool IsRoot { get; set; }
    public string Name { get; set; } = null!;
    public DateTime BirthDate { get; set; }

    public static Person Create(string name, string birthDate)
    {
        return new Person(name, DateTime.Parse(birthDate));
    }

    public void MarkAsRoot()
    {
        IsRoot = true;
    }
}