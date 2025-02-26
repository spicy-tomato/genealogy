using Genealogy.Domain.Postgres.Models;
using Genealogy.Infrastructure.Postgres.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Genealogy.Infrastructure.Postgres.Persistence;

public class PgDbContext(DbContextOptions<PgDbContext> options, IOptions<WhiteList> whiteListEmails)
    : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder
            .UseAsyncSeeding(async (context, _, cancellationToken) =>
            {
                List<string> userEmails =await context.Set<User>()
                    .Select(u => u.Email)
                    .ToListAsync(cancellationToken);
                IEnumerable<string> emailsToSeed = whiteListEmails.Value.Emails
                    .Where(e => !userEmails.Contains(e))
                    .ToList();

                if (!emailsToSeed.Any())
                {
                    return;
                }

                List<User> usersToSeed = emailsToSeed.Select(e => User.Create(Guid.NewGuid(), e, e))
                    .ToList();
                await context.Set<User>().AddRangeAsync(usersToSeed, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            });
    }
}