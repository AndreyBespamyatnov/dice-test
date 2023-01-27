namespace URLShortener.Application.Tests.Queries;

[TestFixture]
public class GetShortUrlQueryTests
{
    private IUrlRepository _repository = null!;
    private GetShortUrlQueryHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _repository = Substitute.For<IUrlRepository>();
        _handler = new GetShortUrlQueryHandler(_repository);
    }

    [Test]
    public async Task Handle_ShouldReturnOriginalUrl_WhenShortUrlExists()
    {
        // Arrange
        const string shortUrl = "abcdefg";
        const string originalUrl = "https://example.com";
        var url = new Url { ShortUrl = shortUrl, OriginalUrl = originalUrl };
        _repository.GetByShortUrlAsync(shortUrl, CancellationToken.None).Returns(url);

        // Act
        var response = await _handler.Handle(new GetShortUrlQuery { ShortUrl = shortUrl }, CancellationToken.None);

        // Assert
        Assert.AreEqual(originalUrl, response.OriginalUrl);
    }

    [Test]
    public void Handle_ShouldThrowNotFoundException_WhenShortUrlDoesNotExist()
    {
        // Arrange
        const string shortUrl = "abcdefg";
        _repository.GetByShortUrlAsync(shortUrl, CancellationToken.None).Returns((Url)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(new GetShortUrlQuery { ShortUrl = shortUrl }, CancellationToken.None));
        Assert.AreEqual($"Url for short '{shortUrl}' not found", ex.Message);
    }
}
