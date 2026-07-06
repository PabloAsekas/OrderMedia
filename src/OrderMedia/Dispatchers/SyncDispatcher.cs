using System;
using Microsoft.Extensions.DependencyInjection;
using OrderMedia.Interfaces.Dispatchers;
using OrderMedia.Interfaces.Handlers;
using OrderMedia.Models;

namespace OrderMedia.Dispatchers;

public class SyncDispatcher : ISyncDispatcher
{
    private readonly IServiceProvider _provider;

    public SyncDispatcher(IServiceProvider provider)
    {
        _provider = provider;
    }

    public void Dispatch(SyncRequest request)
    {
        var handlerType = typeof(ISyncHandler<>)
            .MakeGenericType(request.GetType());

        dynamic handler = _provider.GetRequiredService(handlerType);

        handler.Execute((dynamic)request);
    }
}