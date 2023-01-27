using FluentValidation;

namespace URLShortener.Application.Queries;

public class GetShortUrlQueryValidator : AbstractValidator<GetShortUrlQuery>
{
    public GetShortUrlQueryValidator()
    {
        RuleFor(x => x.ShortUrl).NotEmpty().WithMessage("Short url is required.");
    }
}