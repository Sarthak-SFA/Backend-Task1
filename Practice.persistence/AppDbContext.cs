using Microsoft.EntityFrameworkCore;
namespace Practice.Persistence;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<State> State => Set<State>();

}
