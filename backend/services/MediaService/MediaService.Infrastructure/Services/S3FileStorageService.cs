using Amazon.S3;
using Amazon.S3.Model;
using MediaService.Application;
using MediaService.Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Shared.Common.Application;

namespace MediaService.Infrastructure.Services;

public class S3FileStorageService : IFileStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public S3FileStorageService(IAmazonS3 s3Client, IConfiguration configuration)
    {
        _s3Client = s3Client;
        _bucketName = configuration["AWS:S3:BucketName"] ?? "media-bucket";
    }

    public async Task<Result<FileUploadResult>> UploadFileAsync(
        Stream fileStream,
        string fileName,
        string contentType,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName,
                InputStream = fileStream,
                ContentType = contentType,
                CannedACL = S3CannedACL.PublicRead
            };

            var response = await _s3Client.PutObjectAsync(request, cancellationToken);

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                return Result<FileUploadResult>.Failure("Failed to upload file to S3");
            }

            var publicUrl = $"https://{_bucketName}.s3.amazonaws.com/{fileName}";
            var result = new FileUploadResult(fileName, publicUrl);
            
            return Result<FileUploadResult>.Success(result);
        }
        catch (AmazonS3Exception ex)
        {
            return Result<FileUploadResult>.Failure($"S3 error: {ex.Message}");
        }
        catch (Exception ex)
        {
            return Result<FileUploadResult>.Failure($"Error uploading file: {ex.Message}");
        }
    }

    public async Task<Result<Stream>> DownloadFileAsync(
        string storagePath,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = storagePath
            };

            var response = await _s3Client.GetObjectAsync(request, cancellationToken);
            
            var memoryStream = new MemoryStream();
            await response.ResponseStream.CopyToAsync(memoryStream, cancellationToken);
            memoryStream.Position = 0;

            return Result<Stream>.Success(memoryStream);
        }
        catch (AmazonS3Exception ex)
        {
            return Result<Stream>.Failure($"S3 error: {ex.Message}");
        }
        catch (Exception ex)
        {
            return Result<Stream>.Failure($"Error downloading file: {ex.Message}");
        }
    }

    public async Task<Result> DeleteFileAsync(
        string storagePath,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = storagePath
            };

            await _s3Client.DeleteObjectAsync(request, cancellationToken);
            
            return Result.Success();
        }
        catch (AmazonS3Exception ex)
        {
            return Result.Failure($"S3 error: {ex.Message}");
        }
        catch (Exception ex)
        {
            return Result.Failure($"Error deleting file: {ex.Message}");
        }
    }

    public async Task<Result<string>> GetPublicUrlAsync(
        string storagePath,
        CancellationToken cancellationToken = default)
    {
        var publicUrl = $"https://{_bucketName}.s3.amazonaws.com/{storagePath}";
        return Result<string>.Success(publicUrl);
    }
}
