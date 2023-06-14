using System;
using OrderMedia.Interfaces;

namespace OrderMedia.Services
{
    public class RandomizerService : IRandomizerService
	{
        public string GetRandomNumberAsD4()
        {
            return new Random().Next(0, 9999).ToString("D4");
        }
    }
}

