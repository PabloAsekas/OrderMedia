using System.Text.Json.Serialization;

namespace OrderMedia.Immich.Dtos;

public class AssetDto
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("type")]
    public required string Type { get; init; }

    [JsonPropertyName("originalPath")]
    public string? OriginalPath { get; init; }

    [JsonPropertyName("originalFileName")]
    public string? OriginalFileName { get; init; }

    [JsonPropertyName("livePhotoVideoId")]
    public string? LivePhotoVideoId { get; init; }
}