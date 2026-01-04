using MediatR;
using MediaService.Contracts.DTOs;
using Shared.Common.Application;

namespace MediaService.Application.Commands;

public record GenerateThumbnailCommand(
    int MediaFileId,
    int Width = 200,
    int Height = 200,
    bool MaintainAspectRatio = true
) : IRequest<Result<ImageMetadataDto>>;
