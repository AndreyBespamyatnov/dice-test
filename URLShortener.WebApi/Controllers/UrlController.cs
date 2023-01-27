using Microsoft.AspNetCore.Mvc;
using URLShortener.Application.Queries;
using URLShortener.Application.Requests;
using URLShortener.Domain.Models;

namespace URLShortener.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UrlController : ApiControllerBase
{
    [HttpPost]
    public async Task<CreateShortUrlResponse> Create([FromBody] CreateShortUrlRequest request) => 
        await Mediator.Send(request);

    [HttpGet]
    public async Task<ListUrlsResponse<Url>> Get(int pageNumber, int pageSize) => 
        await Mediator.Send(new ListUrlsQuery { PageNumber = pageNumber, PageSize = pageSize });

    [HttpGet("~/{shortUrl}")]
    public async Task<RedirectResult> Lookup(string shortUrl)
    {
        var response = await Mediator.Send(new GetShortUrlQuery
        {
            ShortUrl = shortUrl
        });
        return Redirect(response.OriginalUrl ?? throw new InvalidOperationException());
    }
}