using Shared.Common.Application;

namespace MediaService.Application;

public interface IFileStorageService
{
    Task<Result<FileUploadResult>> UploadFileAsync(
        Stream fileStream,
        string fileName,
        string contentType,
        CancellationToken cancellationToken = default);

    Task<Result<Stream>> DownloadFileAsync(
        string storagePath,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteFileAsync(
        string storagePath,
        CancellationToken cancellationToken = default);

    Task<Result<string>> GetPublicUrlAsync(
        string storagePath,
        CancellationToken cancellationToken = default);
}

public record FileUploadResult(
    string StoragePath,
    string? PublicUrl
);
