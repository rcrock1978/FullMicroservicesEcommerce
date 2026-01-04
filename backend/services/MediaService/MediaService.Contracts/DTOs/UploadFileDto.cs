using MediaService.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace MediaService.Contracts.DTOs;

public record UploadFileDto(
    IFormFile File,
    MediaType? MediaType = null,
    int? EntityId = null,
    string? EntityType = null
);
