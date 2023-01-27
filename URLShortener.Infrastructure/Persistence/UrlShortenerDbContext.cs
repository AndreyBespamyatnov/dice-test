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
}