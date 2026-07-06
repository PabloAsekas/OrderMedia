using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderMedia.ConsoleApp.Configuration;
using OrderMedia.ConsoleApp.Extensions;
using OrderMedia.ConsoleApp.Interfaces;
using OrderMedia.DigiKam.Interfaces;
using OrderMedia.DigiKam.Models;
using OrderMedia.Immich.Interfaces;
using OrderMedia.Interfaces.Dispatchers;
using OrderMedia.Models;

namespace OrderMedia.ConsoleApp.Orchestrators;

public class SynchronizationOrchestrator : IOrchestrator
{
    public const string ServiceName = "SynchronizationOrchestrator";
    
    private readonly ILogger<SynchronizationOrchestrator> _logger;
    private readonly SynchronizationSettings _synchronizationSettings;
    private readonly IDigiKamService _digiKamService;
    private readonly IImmichService _immichService;
    private readonly ISyncDispatcher _syncDispatcher;
    private readonly ISyncExclusionService _syncExclusionService;

    public SynchronizationOrchestrator(
        ILogger<SynchronizationOrchestrator> logger,
        IOptions<SynchronizationSettings> synchronizationSettingsOptions,
        IDigiKamService digiKamService,
        IImmichService immichService, 
        ISyncDispatcher syncDispatcher, 
        ISyncExclusionService syncExclusionService)
    {
        _logger = logger;
        _synchronizationSettings = synchronizationSettingsOptions.Value;
        _digiKamService = digiKamService;
        _immichService = immichService;
        _syncDispatcher = syncDispatcher;
        _syncExclusionService = syncExclusionService;
    }
    
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        _logger.StartSynchronization();

        var request = CreateRequest(_synchronizationSettings.RequestRules);
        
        var digiKamMedia = (await _digiKamService.GetMediaRelativePathsAsync(request, cancellationToken)).ToList();

        
        var immichMedia = (await _immichService.GetPathsAsync(cancellationToken)).ToList();

        var toAdd = _syncExclusionService.Exclude(digiKamMedia, immichMedia).ToList();
        var toRemove = _syncExclusionService.Exclude(immichMedia,  digiKamMedia).ToList();

        _syncDispatcher.Dispatch(new AddSyncRequest(
            toAdd,
            _synchronizationSettings.SourcePath,
            _synchronizationSettings.DestinationPath
        ));
        

        _syncDispatcher.Dispatch(new RemoveSyncRequest(
            toRemove,
            _synchronizationSettings.DestinationPath
        ));
        
        _logger.EndSyncrhonization();
    }

    private static GetMediaPathsRequest CreateRequest(List<RequestRule> requestRules)
    {
        return new GetMediaPathsRequest
        {
            Requests = requestRules.Select(x => new RequestDetails()
            {
                ExcludingTags = x.ExcludingTags.ToList(),
                Format = Enum.Parse<Format>(x.Format),
                IncludingTags = x.IncludingTags.ToList(),
            }).ToList()
        };
    }
}