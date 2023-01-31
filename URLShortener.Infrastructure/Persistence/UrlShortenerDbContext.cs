using Microsoft.EntityFrameworkCore;
using URLShortener.Domain.Models;

namespace URLShortener.Infrastructure.Persistence;

public class UrlShortenerDbContext : DbContext, IUrlShortenerDbContext
{
    public UrlShortenerDbContext()
    {
    }

    public UrlShortenerDbContext(DbContextOptions<UrlShortenerDbContext> options)
        : base(options) { }

    public DbSet<Url> Urls { get; init; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Url>()
            .HasIndex(x => x.CreatedAt)
            .IsDescending()
            ;
        
        modelBuilder.Entity<Url>()
            .HasKey(x => x.ShortUrl)
            ;
        
        modelBuilder.Entity<Url>()
            .HasIndex(x => x.ShortUrl)
            .IsUnique()
            ;
    }
}