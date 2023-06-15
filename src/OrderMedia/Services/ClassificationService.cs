using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.Services
{
    /// <summary>
    /// Classification service.
    /// </summary>
	public class ClassificationService : IClassificationService
    {
        private readonly IProcessorFactory _processorFactory;

        public ClassificationService(IProcessorFactory processorFactory)
        {
            _processorFactory = processorFactory;
        }

        public void Process(Media media)
        {
            var processor = _processorFactory.CreateProcessor(media.MediaType);

            processor.Execute(media);
        }
    }
}

