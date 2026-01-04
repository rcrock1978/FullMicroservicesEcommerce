using MediatR;
using MediaService.Contracts.DTOs;
using MediaService.Domain.Enums;
using Shared.Common.Application;

namespace MediaService.Application.Commands;

public record UploadFileCommand(
    Stream FileStream,
    string FileName,
    string ContentType,
    long FileSize,
    int UserId,
    MediaType? MediaType = null,
    int? EntityId = null,
    string? EntityType = null,
    StorageProvider StorageProvider = StorageProvider.S3
) : IRequest<Result<MediaFileDto>>;
