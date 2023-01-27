using Microsoft.EntityFrameworkCore;
using URLShortener.Application;
using URLShortener.Domain.Models;

namespace URLShortener.Infrastructure.Persistence;

public class UrlRepository : IUrlRepository
{
    private readonly UrlShortenerDbContext _context;

    public UrlRepository(UrlShortenerDbContext context)
    {
        _context = context;
    }

    public async Task<Url?> GetByOriginalUrlAsync(string originalUrl, CancellationToken cancellationToken)
    {
        var url = await _context.Urls.FirstOrDefaultAsync(x => x.OriginalUrl == originalUrl, cancellationToken: cancellationToken);
        return url;
    }

    public async Task<(List<Url> urls, int totalCount)> GetUrls(int skip, int take, CancellationToken cancellationToken)
    {
        var urls = await _context.Urls
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken: cancellationToken);

        var totalCount = await _context.Urls.CountAsync(cancellationToken: cancellationToken);

        return (urls, totalCount);
    }

    public async Task<Url?> GetByShortUrlAsync(string shortUrl, CancellationToken cancellationToken)
    {
        var url = await _context.Urls.FirstOrDefaultAsync(x => x.ShortUrl == shortUrl, cancellationToken: cancellationToken);
        return url;
    }

    public async Task<Url> AddAsync(Url url, CancellationToken cancellationToken)
    {
        await _context.Urls.AddAsync(url, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return url;
    }

    public async Task UpdateAsync(Url url, CancellationToken cancellationToken)
    {
        _context.Urls.Update(url);
        await _context.SaveChangesAsync(cancellationToken);
    }

    // TODO: we do not use it for now, but we may want to remove records in future
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var url = await _context.Urls.FindAsync(id, cancellationToken);
        _context.Urls.Remove(url);
        await _context.SaveChangesAsync(cancellationToken);
    }
}