using OrderMedia.Models;

namespace OrderMedia.Interfaces.Handlers;

public interface ISyncHandler<in TSyncRequest>
    where TSyncRequest : SyncRequest
{
    void Execute(TSyncRequest request);
}