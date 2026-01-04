using FluentValidation;
using MediaService.Application.Commands;

namespace MediaService.Application.Validators;

public class GenerateThumbnailCommandValidator : AbstractValidator<GenerateThumbnailCommand>
{
    public GenerateThumbnailCommandValidator()
    {
        RuleFor(x => x.MediaFileId)
            .GreaterThan(0)
            .WithMessage("Media file ID must be greater than 0");

        RuleFor(x => x.Width)
            .GreaterThan(0)
            .WithMessage("Width must be greater than 0")
            .LessThanOrEqualTo(4096)
            .WithMessage("Width cannot exceed 4096 pixels");

        RuleFor(x => x.Height)
            .GreaterThan(0)
            .WithMessage("Height must be greater than 0")
            .LessThanOrEqualTo(4096)
            .WithMessage("Height cannot exceed 4096 pixels");
    }
}
