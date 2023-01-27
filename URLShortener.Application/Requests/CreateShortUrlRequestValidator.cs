using FluentValidation;

namespace URLShortener.Application.Requests
{
    public class CreateShortUrlRequestValidator : AbstractValidator<CreateShortUrlRequest>
    {
        public CreateShortUrlRequestValidator()
        {
            RuleFor(x => x.OriginalUrl)
                .NotEmpty()
                .WithMessage("OriginalUrl is required.")
                .MaximumLength(2000)
                .WithMessage("OriginalUrl must be less than 2000 characters.")
                .Must(x => Uri.IsWellFormedUriString(x, UriKind.Absolute))
                .WithMessage("OriginalUrl must be a valid URL.");
        }
    }
}