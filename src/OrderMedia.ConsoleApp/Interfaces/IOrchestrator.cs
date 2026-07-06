namespace OrderMedia.ConsoleApp.Interfaces;

/// <summary>
/// Orchestrator.
/// </summary>
public interface IOrchestrator
{
    /// <summary>
    /// Runs orchestration.
    /// </summary>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Task.</returns>
    Task RunAsync(CancellationToken cancellationToken);
}