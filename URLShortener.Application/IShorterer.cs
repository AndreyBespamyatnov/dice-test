namespace URLShortener.Application;

public interface IShortener
{
    string GenerateShortUrl(string url);
}