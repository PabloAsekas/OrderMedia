using System.Collections.Generic;

namespace OrderMedia.Models;

public sealed record AddSyncRequest(
    List<string> RelativePaths,
    string SourcePath,
    string DestinationPath
    ) : SyncRequest(RelativePaths);