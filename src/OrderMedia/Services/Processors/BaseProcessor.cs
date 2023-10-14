using System.Collections.Generic;
using OrderMedia.Interfaces;
using OrderMedia.Models;

namespace OrderMedia.Services.Processors
{
	public abstract class BaseProcessor : IProcessor
	{
		protected List<IProcessor> processors = new List<IProcessor>();

		public abstract void Execute(Media media);

        public void AddProcessor(IProcessor processor)
		{
			processors.Add(processor);
		}

		protected void ExecuteProcessors(Media media)
		{
			foreach(var processor in processors)
			{
				processor.Execute(media);
			}
		}
	}
}

