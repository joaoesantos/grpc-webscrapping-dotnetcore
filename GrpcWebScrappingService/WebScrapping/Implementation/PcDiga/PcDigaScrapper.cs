using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp.ProcessingData;
using ConsoleApp.WebScrapping.Definition;
using OpenQA.Selenium;

namespace ConsoleApp.WebScrapping.Implementation.PcDiga
{
    public class PcDigaScrapper : AbstractScrapper
    {
        public PcDigaScrapper(long id, string url, DataProcessor dataProcessor) : base(id, url, dataProcessor) {}

        public override async void Run()
        {
            FirefoxDriver.Navigate().GoToUrl(Url);
            ReadOnlyCollection<IWebElement> elements = FirefoxDriver.FindElementsByClassName("product-item");
            TaskFactory taskFactory = new TaskFactory();
            IEnumerable<Task> tasks = elements.Select( el => 
            taskFactory.StartNew(() =>
             {
                 bool inStock = "em stock"
                     .Equals(
                         el.FindElement(By.ClassName("skrey_estimate_date_wrapper")).Text.ToLower());
                 string itemName = el.FindElement(
                         By.ClassName("product-item-name"))
                     .FindElement(
                         By.ClassName("product-item-link"))
                     .Text;
                 string price = el.FindElement(By.ClassName("price")).Text
                     .Replace(",", ".").Replace(" ", String.Empty);
                 PcDigaScrappingData pcDigaScrappingData =
                     new PcDigaScrappingData(
                         Url,
                         inStock,
                         itemName,
                         Convert.ToDouble(price.Substring(0, price.Length - 2)),
                         Id
                     );
                 DataProcessor.PublishData(pcDigaScrappingData);
             }));
            await Task.WhenAll(tasks.ToArray());
            FirefoxDriver.Quit();
        }

        public override void OnComplete(IScrappingData scrappingData)
        {
            throw new System.NotImplementedException();
        }
    }
}