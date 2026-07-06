using System.Collections.Generic;

namespace OrderMedia.Models;

public sealed record RemoveSyncRequest(
    List<string> RelativePaths,
    string DestinationPath
    ) : SyncRequest(RelativePaths);