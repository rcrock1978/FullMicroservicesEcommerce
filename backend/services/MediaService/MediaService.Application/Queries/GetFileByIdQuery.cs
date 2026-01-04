using MediatR;
using MediaService.Contracts.DTOs;
using Shared.Common.Application;

namespace MediaService.Application.Queries;

public record GetFileByIdQuery(int MediaFileId) : IRequest<Result<MediaFileDto>>;
