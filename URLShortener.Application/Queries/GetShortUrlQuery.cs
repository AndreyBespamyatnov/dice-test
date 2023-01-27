using MediatR;
using URLShortener.Application.Exceptions;

namespace URLShortener.Application.Queries;

public class GetShortUrlQuery : IRequest<ShortUrlResponse>
{
    public string ShortUrl { get; init; } = null!;
}

public class ShortUrlResponse
{
    public string? OriginalUrl { get; init; }
}

public class GetShortUrlQueryHandler : IRequestHandler<GetShortUrlQuery, ShortUrlResponse>
{
    private readonly IUrlRepository _repository;

    public GetShortUrlQueryHandler(IUrlRepository repository)
    {
        _repository = repository;
    }

    public async Task<ShortUrlResponse> Handle(GetShortUrlQuery request, CancellationToken cancellationToken)
    {
        var url = await _repository.GetByShortUrlAsync(request.ShortUrl, cancellationToken);
        if (url == null)
        {
            throw new NotFoundException($"Url for short '{request.ShortUrl}' not found");
        }

        return new ShortUrlResponse
        {
            OriginalUrl = url.OriginalUrl
        };
    }
}