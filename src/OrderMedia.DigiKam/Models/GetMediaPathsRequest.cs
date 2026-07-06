namespace OrderMedia.DigiKam.Models;

public class GetMediaPathsRequest
{
    public List<RequestDetails> Requests { get; set; } = new();
}

public class RequestDetails
{
    public Format Format { get; set; }
    public List<string> IncludingTags { get; set; } = [];
    public List<string> ExcludingTags { get; set; } = [];
}