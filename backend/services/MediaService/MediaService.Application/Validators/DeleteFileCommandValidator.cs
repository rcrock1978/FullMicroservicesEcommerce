using FluentValidation;
using MediaService.Application.Commands;

namespace MediaService.Application.Validators;

public class DeleteFileCommandValidator : AbstractValidator<DeleteFileCommand>
{
    public DeleteFileCommandValidator()
    {
        RuleFor(x => x.MediaFileId)
            .GreaterThan(0)
            .WithMessage("Media file ID must be greater than 0");

        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("User ID must be greater than 0");
    }
}
