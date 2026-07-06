namespace OrderMedia.DigiKam.Services;

public static class RelativePathsSanitizer
{
    extension(IEnumerable<string> relativePaths)
    {
        public IEnumerable<string> Sanitize()
        {
            return relativePaths.Select(x => x.StartsWith('/') ? x.Remove(0, 1) : x);
        }
    }

}