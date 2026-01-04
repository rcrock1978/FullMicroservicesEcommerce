using MediatR;
using Shared.Common.Application;

namespace MediaService.Application.Commands;

public record DeleteFileCommand(
    int MediaFileId,
    int UserId
) : IRequest<Result>;
