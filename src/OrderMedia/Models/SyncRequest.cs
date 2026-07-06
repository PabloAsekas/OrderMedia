using System.Collections.Generic;

namespace OrderMedia.Models;

public abstract record SyncRequest(
    List<string> RelativePaths
    );