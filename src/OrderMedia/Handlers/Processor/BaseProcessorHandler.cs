using OrderMedia.Interfaces.Handlers;
using OrderMedia.Models;

namespace OrderMedia.Handlers.Processor;

public abstract class BaseProcessorHandler : IProcessorHandler
{
    private IProcessorHandler _nextHandler;

    public IProcessorHandler SetNext(IProcessorHandler handler)
    {
        _nextHandler = handler;

        return handler;
    }

    public virtual void Process(Media media)
    {
        _nextHandler?.Process(media);
    }
}