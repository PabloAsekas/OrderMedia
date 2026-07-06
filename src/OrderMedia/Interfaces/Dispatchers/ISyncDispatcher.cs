using OrderMedia.Models;

namespace OrderMedia.Interfaces.Dispatchers;

public interface ISyncDispatcher
{
    void Dispatch(SyncRequest request);
}