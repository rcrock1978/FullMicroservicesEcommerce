using FluentValidation;
using MediaService.Application.Commands;

namespace MediaService.Application.Validators;

public class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
{
    private const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10 MB
    private static readonly string[] AllowedImageTypes = { "image/jpeg", "image/png", "image/gif", "image/webp", "image/svg+xml" };
    private static readonly string[] AllowedVideoTypes = { "video/mp4", "video/mpeg", "video/webm" };
    private static readonly string[] AllowedDocumentTypes = { "application/pdf", "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" };

    public UploadFileCommandValidator()
    {
        RuleFor(x => x.FileStream)
            .NotNull()
            .WithMessage("File stream is required");

        RuleFor(x => x.FileName)
            .NotEmpty()
            .WithMessage("File name is required")
            .MaximumLength(255)
            .WithMessage("File name cannot exceed 255 characters");

        RuleFor(x => x.ContentType)
            .NotEmpty()
            .WithMessage("Content type is required")
            .Must(BeValidContentType)
            .WithMessage("Unsupported content type");

        RuleFor(x => x.FileSize)
            .GreaterThan(0)
            .WithMessage("File size must be greater than 0")
            .LessThanOrEqualTo(MaxFileSizeBytes)
            .WithMessage($"File size cannot exceed {MaxFileSizeBytes / 1024 / 1024} MB");

        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("User ID must be greater than 0");

        When(x => x.EntityId.HasValue, () =>
        {
            RuleFor(x => x.EntityId!.Value)
                .GreaterThan(0)
                .WithMessage("Entity ID must be greater than 0");
        });

        When(x => x.EntityId.HasValue, () =>
        {
            RuleFor(x => x.EntityType)
                .NotEmpty()
                .WithMessage("Entity type is required when entity ID is provided");
        });
    }

    private bool BeValidContentType(string contentType)
    {
        var allAllowedTypes = AllowedImageTypes
            .Concat(AllowedVideoTypes)
            .Concat(AllowedDocumentTypes);

        return allAllowedTypes.Contains(contentType.ToLower());
    }
}
