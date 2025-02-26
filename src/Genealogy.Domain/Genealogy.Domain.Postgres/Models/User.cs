using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace Genealogy.Domain.Postgres.Models;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class User
{
    private User(Guid id, string name, string email)
    {
        Id = id;
        Name = name;
        Email = email;
    }

    public Guid Id { get; init; }

    [MaxLength(100)]
    public string Name { get; init; }

    [MaxLength(100)]
    public string Email { get; init; }

    public static User Create(Guid id, string name, string email)
    {
        return new User(id, name, email);
    }
}