using System.Security.Cryptography;
using System.Text;
using URLShortener.Application;

namespace URLShortener.Infrastructure;

public class Shortener: IShortener
{
    public string GenerateShortUrl(string url)
    {
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(url));
        var shortUrl = Convert.ToBase64String(hash)[..6];
        return shortUrl;
    }
}