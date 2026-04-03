using System.Security.Cryptography.X509Certificates;
using FluentValidation;
using TonnUr.Application.Communities.Commands.CreateCommunity;

public sealed class CreateCommunityCommandValidator : AbstractValidator<CreateCommunityCommand>
{
    public CreateCommunityCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(240)
            .WithMessage("Name must be less than 240 characters");
        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("Description must be less than 1000 characters");
    
    }
}