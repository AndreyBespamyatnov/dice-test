using StackExchange.Redis;
using URLShortener.Application;

namespace URLShortener.Infrastructure.Cache;

public class RedisCache : ICache
{
    private readonly IConnectionMultiplexer _redis;

    public RedisCache(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task<bool> SetAsync(string key, string value, TimeSpan? ttl = null)
    {
        ttl ??= TimeSpan.FromMinutes(1);
        var db = _redis.GetDatabase();
        return await db.StringSetAsync(key, value, ttl);
    }

    public async Task<(bool found, string? value)> TryGetValue(string key)
    {
        var redisValue = await GetRedisValue(key);
        return !redisValue.HasValue ? (false, default) : (true, redisValue);
    }

    public async Task<bool> RemoveAsync(string key)
    {
        var db = _redis.GetDatabase();
        return await db.KeyDeleteAsync(key);
    }

    private async Task<RedisValue> GetRedisValue(string key)
    {
        var db = _redis.GetDatabase();
        return await db.StringGetAsync(key);
    }
}