using MediatR;
using URLShortener.Application;
using URLShortener.Application.Queries;
using URLShortener.Application.Requests;

namespace URLShortener.Infrastructure.Behaviors;

public class RedisCachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    private readonly ICache _cache;

    public RedisCachingBehavior(ICache cache)
    {
        _cache = cache;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        switch (request)
        {
            case GetShortUrlQuery query:
            {
                var (found, value) = await _cache.TryGetValue(query.ShortUrl);
                if (found && !string.IsNullOrWhiteSpace(value))
                {
                    return (TResponse)(object)new ShortUrlResponse { OriginalUrl = value };
                }

                var response = await next();
                await _cache.SetAsync(query.ShortUrl, (response as ShortUrlResponse).OriginalUrl);
                return response;
            }
            case CreateShortUrlRequest command:
            {
                var response = await next();
                await _cache.SetAsync((response as CreateShortUrlResponse).Url.ShortUrl, command.OriginalUrl);
                return response;
            }
            default:
            {
                return await next();
            }
        }
    }
}