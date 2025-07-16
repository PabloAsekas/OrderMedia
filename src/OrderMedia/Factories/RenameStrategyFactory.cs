using System;
using OrderMedia.Enums;
using OrderMedia.Interfaces;
using OrderMedia.Interfaces.Factories;
using OrderMedia.Strategies.RenameStrategy;

namespace OrderMedia.Factories;

/// <summary>
/// Rename Strategy Factory. To create <<see cref="IRenameStrategy"/> by the <see cref="MediaType"/>.
/// </summary>
public class RenameStrategyFactory : IRenameStrategyFactory
{
    private readonly DefaultRenameStrategy _defaultRenameStrategy;
    private readonly Insta360RenameStrategy _insta360RenameStrategy;

    public RenameStrategyFactory(DefaultRenameStrategy defaultRenameStrategy, Insta360RenameStrategy insta360RenameStrategy)
    {
        _defaultRenameStrategy = defaultRenameStrategy;
        _insta360RenameStrategy = insta360RenameStrategy;
    }

    public IRenameStrategy GetRenameStrategy(MediaType mediaType)
    {
        return mediaType switch
        {
            MediaType.Image => _defaultRenameStrategy,
            MediaType.Raw => _defaultRenameStrategy,
            MediaType.Video => _defaultRenameStrategy,
            MediaType.WhatsAppImage => _defaultRenameStrategy,
            MediaType.WhatsAppVideo => _defaultRenameStrategy,
            MediaType.Insv => _insta360RenameStrategy,
            _ => throw new FormatException($"The provided media type '{mediaType}' is not supported.")
        };
    }
}