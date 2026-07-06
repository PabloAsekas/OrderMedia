using OrderMedia.Immich.Dtos;
using OrderMedia.Immich.Interfaces;

namespace OrderMedia.Immich.Services;

public class ImmichService : IImmichService
{
    private readonly IImmichClient _immichClient;

    public ImmichService(IImmichClient immichClient)
    {
        _immichClient = immichClient;
    }

    public async Task<IEnumerable<string>> GetPathsAsync(CancellationToken cancellationToken = default)
    {
        var results = new List<string>();
        
        await foreach (var asset in _immichClient.GetAllAssetsAsync(cancellationToken: cancellationToken))
        {
            // Aquí hay que sanitizar el path.
            results.Add(asset.OriginalPath.Remove(0, "/mnt/photos/pablo/".Length));

            if (string.IsNullOrWhiteSpace(asset.LivePhotoVideoId))
            {
                continue;
            }
            
            results.Add(await HandleLivePhotoAsync(asset, cancellationToken));
            
            /*var video = await _immichClient.GetAssetByIdAsync(asset.LivePhotoVideoId, cancellationToken);

            if (video == null)
            {
                continue;
            }

            if (video.OriginalPath.StartsWith("/data/encoded-video/"))
            {
                continue;
            }
            
            results.Add(video.OriginalPath.Remove(0, "/mnt/photos/pablo/".Length));*/
        }
        
        var list = results.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
        
        return results.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
    }

    private async Task<string> HandleLivePhotoAsync(AssetDto asset, CancellationToken cancellationToken)
    {
        var video = await _immichClient.GetAssetByIdAsync(asset.LivePhotoVideoId!, cancellationToken);

        return video.OriginalPath!.StartsWith("/data/encoded-video/") ? string.Empty : video.OriginalPath.Remove(0, "/mnt/photos/pablo/".Length);
    }
}