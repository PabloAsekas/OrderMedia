using OrderMedia.Interfaces.Handlers;

namespace OrderMedia.Interfaces.Factories;

public interface ICreatedDateChainFactory
{
    ICreatedDateHandler CreateChain();
}