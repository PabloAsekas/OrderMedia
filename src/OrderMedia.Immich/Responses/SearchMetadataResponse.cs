using System.Text.Json.Serialization;
using OrderMedia.Immich.Dtos;

namespace OrderMedia.Immich.Responses;

public sealed class SearchMetadataResponse
{
    [JsonPropertyName("assets")]
    public required SearchAssetsResponse Assets { get; init; }
}

public sealed class SearchAssetsResponse
{
    [JsonPropertyName("items")]
    public required List<AssetDto> Items { get; init; }

    [JsonPropertyName("total")]
    public int Total { get; init; }

    [JsonPropertyName("nextPage")]
    public string? NextPage { get; init; }
}