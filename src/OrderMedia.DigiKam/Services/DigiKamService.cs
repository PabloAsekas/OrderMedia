using OrderMedia.DigiKam.Interfaces;
using OrderMedia.DigiKam.Models;

namespace OrderMedia.DigiKam.Services;

public class DigiKamService : IDigiKamService
{
    private readonly IRepository _repository;

    public DigiKamService(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<string>> GetMediaRelativePathsAsync(GetMediaPathsRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Requests.Count == 0)
        {
            return [];
        }

        var allPaths = await Task.WhenAll(request.Requests.Select(x => GetMediaPathsByRequestAsync(x, cancellationToken)));
        
        return allPaths
            .SelectMany(paths => paths)
            .Distinct()
            .OrderBy(x => x)
            .Sanitize()
            .ToList();
    }

    private async Task<IEnumerable<string>> GetMediaPathsByRequestAsync(RequestDetails requestDetails,
        CancellationToken cancellationToken = default)
    {
        var includingTagsTask = GetTagListAsync(requestDetails.IncludingTags, cancellationToken);
        var excludingTagsTask = GetTagListAsync(requestDetails.ExcludingTags, cancellationToken);
            
        await Task.WhenAll(includingTagsTask, excludingTagsTask);

        return await GetPathsByFormatAndTagsAsync(requestDetails.Format, includingTagsTask.Result, excludingTagsTask.Result,
            cancellationToken);
    }
    
    private async Task<IEnumerable<Tag>> GetTagListAsync(List<string> tags, CancellationToken cancellationToken = default)
    {
        var tasks = tags.Select(tagName =>
            _repository.GetTagByTagNameAsync(tagName, cancellationToken));

        var results = await Task.WhenAll(tasks);

        return results
            .Where(tag => tag is not null)
            .Cast<Tag>();
    }
    
    private async Task<IEnumerable<string>> GetPathsByFormatAndTagsAsync(Format format, IEnumerable<Tag> includingTags, IEnumerable<Tag> excludingTags,
        CancellationToken cancellationToken = default)
    {
        var query = new GetMediaPathsQuery
        {
            Format = format,
            IncludingTags = includingTags.ToList(),
            ExcludingTags = excludingTags.ToList()
        };
        
        return await _repository.GetMediaPathsAsync(query, cancellationToken);
    }
}