using Microsoft.EntityFrameworkCore;
using URLShortener.Domain.Models;

namespace URLShortener.Infrastructure.Persistence;

public interface IUrlShortenerDbContext
{
    DbSet<Url> Urls { get; init; }
}