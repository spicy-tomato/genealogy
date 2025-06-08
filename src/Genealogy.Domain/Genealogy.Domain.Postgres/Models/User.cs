using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;

namespace Genealogy.Domain.Postgres.Models;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public sealed class User : IdentityUser
{
    [MaxLength(255)]
    public string? Name { get; set; }
    
    public User(string? name, string email) : base(email)
    {
        Email = email;
        Name = name;
    }

    public static User Create(string? name, string email)
    {
        return new User(name, email);
    }
}