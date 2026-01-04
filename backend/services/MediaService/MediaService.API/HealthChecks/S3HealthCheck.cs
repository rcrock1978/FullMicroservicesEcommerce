using Amazon.S3;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MediaService.API.HealthChecks;

public class S3HealthCheck : IHealthCheck
{
    private readonly IAmazonS3 _s3Client;
    private readonly ILogger<S3HealthCheck> _logger;

    public S3HealthCheck(IAmazonS3 s3Client, ILogger<S3HealthCheck> logger)
    {
        _s3Client = s3Client;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _s3Client.ListBucketsAsync(cancellationToken);
            return HealthCheckResult.Healthy("S3 connection is healthy");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "S3 health check failed");
            return HealthCheckResult.Unhealthy("S3 connection is unhealthy", ex);
        }
    }
}
