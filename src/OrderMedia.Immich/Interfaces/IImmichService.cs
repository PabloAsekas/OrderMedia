namespace OrderMedia.Immich.Interfaces;

public interface IImmichService
{
    public Task<IEnumerable<string>> GetPathsAsync(CancellationToken cancellationToken = default);
}