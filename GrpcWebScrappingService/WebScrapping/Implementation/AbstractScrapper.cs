using System.Threading.Tasks;
using ConsoleApp.ProcessingData;
using ConsoleApp.WebScrapping.Definition;
using ConsoleApp.WebScrapping.Implementation.PcDiga;
using OpenQA.Selenium.Firefox;

namespace ConsoleApp.WebScrapping.Implementation
{
    public abstract class AbstractScrapper : IScrapper
    {
        protected readonly string Url;
        protected readonly DataProcessor DataProcessor;
        protected readonly FirefoxDriver FirefoxDriver;
        protected readonly long Id;
        protected AbstractScrapper(long id, string url, DataProcessor dataProcessor)
        {
            Id = id;
            Url = url;
            DataProcessor = dataProcessor;
            FirefoxDriver = new FirefoxDriver();
        }

        public abstract void Run();
        public abstract void OnComplete(IScrappingData scrappingData);

    }
}