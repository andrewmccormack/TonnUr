using FluentValidation;
using TonnUr.Domain.Communities;

namespace TonnUr.Application.Communities.Commands.CreateCommunity;

public sealed class CreateCommunityCommandValidator : AbstractValidator<CreateCommunityCommand>
{
    public CreateCommunityCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(100)
            .WithMessage("Name must be less than 100 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("Description must be less than 1000 characters");

        When(x => x.Slug is not null, () =>
        {
            RuleFor(x => x.Slug!)
                .MaximumLength(CommunitySlug.MaxLength)
                .WithMessage($"Slug cannot exceed {CommunitySlug.MaxLength} characters")
                .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$")
                .WithMessage("Slug must be lowercase alphanumeric with hyphens only, and cannot start or end with a hyphen");
        });
    }
}
