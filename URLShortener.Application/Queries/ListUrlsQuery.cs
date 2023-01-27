using MediatR;
using URLShortener.Domain.Models;

namespace URLShortener.Application.Queries
{
    public class ListUrlsQuery : IRequest<ListUrlsResponse<Url>>
    {
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
    }

    public class ListUrlsResponse<T>
    {
        public List<T> Data { get; init; }
        public int TotalCount { get; init; }
    }

    public class ListUrlsQueryHandler : IRequestHandler<ListUrlsQuery, ListUrlsResponse<Url>>
    {
        private readonly IUrlRepository _repository;

        public ListUrlsQueryHandler(IUrlRepository repository)
        {
            _repository = repository;
        }

        public async Task<ListUrlsResponse<Url>> Handle(ListUrlsQuery request, CancellationToken cancellationToken)
        {
            var skip = request.PageNumber != 0 ? (request.PageNumber - 1) * request.PageSize : 0;
            var take = request.PageSize;

            var urls = await _repository.GetUrls(skip, take, cancellationToken);

            return new ListUrlsResponse<Url>
            {
                Data = urls.urls,
                TotalCount = urls.totalCount
            };
        }
    }
}
