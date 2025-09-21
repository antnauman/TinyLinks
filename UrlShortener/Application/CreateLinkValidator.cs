using FluentValidation;
using LinkService.Contracts;

namespace LinkService.Application;

public class CreateLinkValidator : AbstractValidator<LinkRequestDTO>
{
    public CreateLinkValidator()
    {
        RuleFor(x => x.TargetUrl).NotEmpty().Must(u => Uri.TryCreate(u, UriKind.Absolute, out _));
        RuleFor(x => x.Threshold).GreaterThan(0).When(x => x.Threshold.HasValue);
    }
}