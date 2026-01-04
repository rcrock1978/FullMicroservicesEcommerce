using MediatR;
using MediaService.Domain.Exceptions;
using MediaService.Domain.Repositories;
using Shared.Common.Application;
using Shared.Common.Domain;

namespace MediaService.Application.Commands;

public class DeleteFileCommandHandler : IRequestHandler<DeleteFileCommand, Result>
{
    private readonly IMediaFileRepository _mediaFileRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageService _fileStorageService;

    public DeleteFileCommandHandler(
        IMediaFileRepository mediaFileRepository,
        IUnitOfWork unitOfWork,
        IFileStorageService fileStorageService)
    {
        _mediaFileRepository = mediaFileRepository;
        _unitOfWork = unitOfWork;
        _fileStorageService = fileStorageService;
    }

    public async Task<Result> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
    {
        var mediaFile = await _mediaFileRepository.GetByIdAsync(request.MediaFileId, cancellationToken);
        
        if (mediaFile == null)
            return Result.Failure("Media file not found");

        // Delete from storage
        var deleteResult = await _fileStorageService.DeleteFileAsync(
            mediaFile.StoragePath,
            cancellationToken);

        if (!deleteResult.IsSuccess)
            return Result.Failure($"Failed to delete file from storage: {deleteResult.Error}");

        // Mark as deleted in database
        mediaFile.MarkAsDeleted();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
