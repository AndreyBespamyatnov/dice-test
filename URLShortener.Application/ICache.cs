namespace URLShortener.Application;

public interface ICache
{
    Task<bool> SetAsync(string key, string value, TimeSpan? ttl = null);
    Task<(bool found, string? value)> TryGetValue(string key);
    Task<bool> RemoveAsync(string key);
}