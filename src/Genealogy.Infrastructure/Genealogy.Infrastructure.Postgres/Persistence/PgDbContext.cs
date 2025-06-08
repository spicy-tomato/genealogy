using Genealogy.Domain.Postgres.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Genealogy.Infrastructure.Postgres.Persistence;

public class PgDbContext(DbContextOptions<PgDbContext> options)
    : IdentityDbContext<User, IdentityRole, string>(options);