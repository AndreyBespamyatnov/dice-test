using NSubstitute;
using URLShortener.Domain.Models;

namespace URLShortener.Application.Tests.Queries;

[TestFixture]
public class ListUrlsQueryHandlerTests
{
    private IUrlRepository _repository = null!;
    private ListUrlsQueryHandler _handler = null!;

    [SetUp]
    public void Setup()
    {
        _repository = Substitute.For<IUrlRepository>();
        _handler = new ListUrlsQueryHandler(_repository);
    }

    [Test]
    public async Task Handle_ShouldReturnListUrlsResponse()
    {
        // Arrange
        var request = new ListUrlsQuery { PageNumber = 2, PageSize = 10 };
        var urls = new List<Url>
        {
            new Url { OriginalUrl = "https://www.example.com", ShortUrl = "abc7fsd" },
            new Url { OriginalUrl = "https://www.example2.com", ShortUrl = "def8qwe" },
        };
        const int totalCount = 20;
        _repository.GetUrls(10, 10, CancellationToken.None).Returns(Task.FromResult((urls, totalCount)));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(2, Is.EqualTo(result.Data.Count));
        Assert.That("https://www.example.com", Is.EqualTo(result.Data[0].OriginalUrl));
        Assert.That("abc7fsd", Is.EqualTo(result.Data[0].ShortUrl));
        Assert.That("https://www.example2.com", Is.EqualTo(result.Data[1].OriginalUrl));
        Assert.That("def8qwe", Is.EqualTo(result.Data[1].ShortUrl));
        Assert.That(20, Is.EqualTo(result.TotalCount));
        _repository.Received(1).GetUrls(10, 10, CancellationToken.None);
    }
}