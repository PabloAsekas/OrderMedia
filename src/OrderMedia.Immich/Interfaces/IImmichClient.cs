using OrderMedia.Immich.Dtos;

namespace OrderMedia.Immich.Interfaces;

public interface IImmichClient
{
    IAsyncEnumerable<AssetDto> GetAllAssetsAsync(int pageSize = 1000,
        CancellationToken cancellationToken = default);
    
    Task<AssetDto> GetAssetByIdAsync(string assetId, CancellationToken cancellationToken = default);
}