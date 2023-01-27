namespace URLShortener.Application.Tests.Requests;

[TestFixture]
public class GetShortUrlQueryTests
{
    private CreateShortUrlRequestHandler _handler = null!;
    private IUrlRepository _repository = null!;
    private IShortener _shortener = null!;

    [SetUp]
    public void SetUp()
    {
        _repository = Substitute.For<IUrlRepository>();
        _shortener = Substitute.For<IShortener>();
        _handler = new CreateShortUrlRequestHandler(_repository, _shortener);
    }

    [Test]
    public async Task Handle_ShouldReturnExistingEntity_WhenOriginalUrlExists()
    {
        // Arrange
        const string originalUrl = "https://www.example.com";
        var existingEntity = new Url
        {
            Id = Guid.NewGuid(),
            OriginalUrl = originalUrl,
            ShortUrl = "abcdefg",
            CreatedAt = DateTimeOffset.Now
        };
        _repository.GetByOriginalUrlAsync(originalUrl, Arg.Any<CancellationToken>()).Returns(existingEntity);

        // Act
        var result = await _handler.Handle(new CreateShortUrlRequest { OriginalUrl = originalUrl }, CancellationToken.None);

        // Assert
        Assert.That(result.Url, Is.EqualTo(existingEntity));
        _repository.Received(1).GetByOriginalUrlAsync(originalUrl, Arg.Any<CancellationToken>());
        _shortener.DidNotReceive().GenerateShortUrl(Arg.Any<string>());
    }

    [Test]
    public async Task Handle_ShouldCreateNewEntity_WhenOriginalUrlDoesNotExist()
    {
        // Arrange
        const string originalUrl = "https://www.example.com";
        const string shortUrl = "abcdefg";
        _repository.GetByOriginalUrlAsync(originalUrl, Arg.Any<CancellationToken>()).Returns((Url)null);
        _repository.AddAsync(Arg.Any<Url>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(new Url
        {
            OriginalUrl = originalUrl,
            ShortUrl = shortUrl
        }));
        _shortener.GenerateShortUrl(originalUrl).Returns(shortUrl);

        // Act
        var result = await _handler.Handle(new CreateShortUrlRequest { OriginalUrl = originalUrl }, CancellationToken.None);

        // Assert
        Assert.That(result.Url, Is.Not.Null);
        Assert.That(result.Url.OriginalUrl, Is.EqualTo(originalUrl));
        Assert.That(result.Url.ShortUrl, Is.EqualTo(shortUrl));
        _repository.Received(1).GetByOriginalUrlAsync(originalUrl, Arg.Any<CancellationToken>());
        _shortener.Received(1).GenerateShortUrl(originalUrl);
    }
}