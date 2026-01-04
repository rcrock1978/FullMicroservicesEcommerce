namespace MediaService.Contracts.DTOs;

public record GenerateThumbnailDto(
    int Width = 200,
    int Height = 200,
    bool MaintainAspectRatio = true
);
