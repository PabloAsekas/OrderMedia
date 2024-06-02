using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.Handlers.CreatedDate;

public abstract class BaseCreatedDateHandler : ICreatedDateHandler
{
    private ICreatedDateHandler _nextHandler;

    public ICreatedDateHandler SetNext(ICreatedDateHandler handler)
    {
        _nextHandler = handler;

        return handler;
    }

    public virtual CreatedDateInfo GetCreatedDateInfo(string mediaPath)
    {
        return _nextHandler?.GetCreatedDateInfo(mediaPath);
    }

    protected static CreatedDateInfo CreateCreatedDateInfo(string createdDate, string format)
    {
        return string.IsNullOrEmpty(createdDate) ? null : new CreatedDateInfo()
        {
            CreatedDate = createdDate,
            Format = format,
        };
    }
}