using MediatR;
using URLShortener.Domain.Models;

namespace URLShortener.Application.Requests;

public class CreateShortUrlRequest : IRequest<CreateShortUrlResponse>
{
    public string OriginalUrl { get; set; } = null!;
}

public class CreateShortUrlResponse
{
    public Url? Url { get; init; }
}

public class CreateShortUrlRequestHandler : IRequestHandler<CreateShortUrlRequest, CreateShortUrlResponse>
{
    private readonly IUrlRepository _repository;
    private readonly IShortener _shortener;

    public CreateShortUrlRequestHandler(IUrlRepository repository, IShortener shortener)
    {
        _repository = repository;
        _shortener = shortener;
    }

    public async Task<CreateShortUrlResponse> Handle(CreateShortUrlRequest request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByOriginalUrlAsync(request.OriginalUrl, cancellationToken)
                     ?? await CreateNewEntity(request, cancellationToken);

        return new CreateShortUrlResponse
        {
            Url = entity
        };
    }

    private async Task<Url> CreateNewEntity(CreateShortUrlRequest request, CancellationToken cancellationToken)
    {
        var shortUrl = _shortener.GenerateShortUrl(request.OriginalUrl);
        var entity = new Url
        {
            Id = Guid.NewGuid(),
            OriginalUrl = request.OriginalUrl,
            ShortUrl = shortUrl,
            CreatedAt = DateTimeOffset.Now
        };
        return await _repository.AddAsync(entity, cancellationToken);
    }
}