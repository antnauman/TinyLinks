using LinkService.Domain;
using Microsoft.EntityFrameworkCore;

namespace LinkService.Infrastructure;

public class LinkDbContext : DbContext
{
    public LinkDbContext(DbContextOptions<LinkDbContext> options) : base(options) { }
    public DbSet<LinkEntity> Links => Set<LinkEntity>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LinkEntity>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Code).IsUnique();
            e.Property(x => x.Code).IsRequired().HasMaxLength(16);
            e.Property(x => x.OwnerSub).IsRequired().HasMaxLength(128);
            e.Property(x => x.TargetUrl).IsRequired().HasMaxLength(2048);
        });
    }
}