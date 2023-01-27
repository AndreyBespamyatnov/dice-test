namespace URLShortener.Application.Tests.Queries;

[TestFixture]
public class GetShortUrlQueryValidatorTests
{
    private GetShortUrlQueryValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new GetShortUrlQueryValidator();
    }

    [Test]
    public void ShortUrl_IsRequired()
    {
        var model = new GetShortUrlQuery
        {
            ShortUrl = null as string
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.ShortUrl);
    }

    [Test]
    public void ShortUrl_IsNotEmpty()
    {
        var model = new GetShortUrlQuery
        {
            ShortUrl = string.Empty
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.ShortUrl);
    }

    [Test]
    public void ShortUrl_IsValid()
    {
        var model = new GetShortUrlQuery
        {
            ShortUrl = "validShortUrl"
        };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.ShortUrl);
    }
}
