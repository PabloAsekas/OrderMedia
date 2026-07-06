using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using OrderMedia.Immich.Dtos;
using OrderMedia.Immich.Interfaces;
using OrderMedia.Immich.Requests;
using OrderMedia.Immich.Responses;

namespace OrderMedia.Immich.Clients;

public class ImmichClient : IImmichClient
{
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public ImmichClient(HttpClient client)
    {
        _client = client;
    }

    public async IAsyncEnumerable<AssetDto> GetAllAssetsAsync(int pageSize = 1000,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var page = 1;

        while (true)
        {
            var request = new SearchMetadataRequest
            {
                Page = page,
                Size = pageSize,
            };

            using var response = await _client.PostAsJsonAsync(
                "/api/search/metadata",
                request,
                JsonOptions,
                cancellationToken);

            response.EnsureSuccessStatusCode();
            
            var result = await response.Content.ReadFromJsonAsync<SearchMetadataResponse>(
                JsonOptions,
                cancellationToken);

            if (result is null)
            {
                yield break;
            }

            if (result.Assets.Items.Count == 0)
            {
                yield break;
            }

            foreach (var asset in result.Assets.Items)
            {
                yield return asset;
            }

            if (string.IsNullOrWhiteSpace(result.Assets.NextPage))
            {
                yield break;
            }
            
            page++;
        }
    }

    public async Task<AssetDto> GetAssetByIdAsync(string assetId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"/api/assets/{assetId}";
        var response = await _client.GetAsync(requestUri, cancellationToken: cancellationToken);
        
        var result = await response.Content.ReadFromJsonAsync<AssetDto>(cancellationToken);
        
        return result;
    }
}