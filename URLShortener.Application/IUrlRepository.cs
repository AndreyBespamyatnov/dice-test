using URLShortener.Domain.Models;

namespace URLShortener.Application;

public interface IUrlRepository
{
    Task<(List<Url> urls, int totalCount)> GetUrls(int skip, int take, CancellationToken cancellationToken);
    Task<Url?> GetByShortUrlAsync(string shortUrl, CancellationToken cancellationToken);
    Task<Url?> GetByOriginalUrlAsync(string originalUrl, CancellationToken cancellationToken);
    Task<Url> AddAsync(Url url, CancellationToken cancellationToken);
    Task UpdateAsync(Url url, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
