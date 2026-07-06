namespace OrderMedia.DigiKam.Models;

public class GetMediaPathsQuery
{
    public Format Format { get; set; }

    public List<Tag> IncludingTags { get; set; } = new();

    public List<Tag> ExcludingTags { get; set; } = new();
}