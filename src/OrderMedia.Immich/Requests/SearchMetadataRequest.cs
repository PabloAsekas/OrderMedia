using System.Text.Json.Serialization;

namespace OrderMedia.Immich.Requests;

public class SearchMetadataRequest
{
    [JsonPropertyName("page")]
    public int Page { get; init; }

    [JsonPropertyName("size")]
    public int Size { get; init; } = 1000;
}