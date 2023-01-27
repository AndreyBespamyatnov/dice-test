namespace URLShortener.Application.Tests.Requests;

[TestFixture]
public class CreateShortUrlRequestValidatorTests
{
    private CreateShortUrlRequestValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new CreateShortUrlRequestValidator();
    }

    [Test]
    public void OriginalUrl_IsRequired()
    {
        var model = new CreateShortUrlRequest
        {
            OriginalUrl = null as string
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.OriginalUrl);
    }

    [Test]
    public void OriginalUrl_IsNotEmpty()
    {
        var model = new CreateShortUrlRequest
        {
            OriginalUrl = string.Empty
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.OriginalUrl);
    }

    [Test]
    public void OriginalUrl_MaximumLength()
    {
        var model = new CreateShortUrlRequest
        {
            OriginalUrl = new string('a', 2001)
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.OriginalUrl);
    }

    [Test]
    public void OriginalUrl_IsInvalidUrl()
    {
        var model = new CreateShortUrlRequest
        {
            OriginalUrl = "invalid url"
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.OriginalUrl);
    }

    [Test]
    public void OriginalUrl_IsValid()
    {
        var model = new CreateShortUrlRequest
        {
            OriginalUrl = "https://example.com"
        };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.OriginalUrl);
    }
}