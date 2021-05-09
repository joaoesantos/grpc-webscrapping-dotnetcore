using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ConsoleApp.Boot;
using ConsoleApp.ProcessingData;
using ConsoleApp.WebScrapping.Definition;
using ConsoleApp.WebScrapping.Implementation;
using Domain;
using GrpcWebScrappingService.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence;

namespace ConsoleApp
{
    /// <summary>
    /// Class responsible to load program characteristics
    /// </summary>
    public class AppBootstrapper
    {
        public ConcurrentQueue<IScrappingData> Queue { get; private set; }
        public ConcurrentDictionary<IScrappingDataIdentifier, int> PreviousReadings { get; private set; }
        public bool WasInitialised { get; private set; }
        
        public List<IScrapper> Scrappers { get; private set; }
        
        public DataProcessor DataProcessor { get; private set; }

       public void Bootstrap()
       {
           WasInitialised = true;
           Queue = new ConcurrentQueue<IScrappingData>();
           PreviousReadings = new ConcurrentDictionary<IScrappingDataIdentifier, int>();
           Scrappers = new List<IScrapper>();
           DataProcessor = new DataProcessor(Queue, PreviousReadings);
           var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false)
               .Build();
           ScrappingSearch scrappingSearch= configuration
               .GetSection("ScrappingSearch").Get<ScrappingSearch>();
           using (WebScrapperDbContext context = new WebScrappingContextFactory().CreateDbContext(Array.Empty<string>()))
           {
               foreach (var search in scrappingSearch.DefinedSearchs)
               {
                   object[] arguments = {search.Id, search.Url, DataProcessor};
                   Type type = Type.GetType($"{search.Scrapper}, {Assembly.GetExecutingAssembly().FullName}");

                   if (type == null)
                   {
                       throw new ArgumentException($"No valid type for {search.Scrapper}");
                   }
                   
                   var newScrapper = Activator
                       .CreateInstance(
                           type, 
                           arguments) as AbstractScrapper;
                   Scrappers.Add(newScrapper); 
                   
                   WebScrappingSearch dbSearch = context.WebScrappingSearches.Find(search.Id);
                   if (dbSearch == null)
                   {
                       WebScrappingSearch newSearch = new WebScrappingSearch
                       {
                           Id = search.Id,
                           Url = search.Url
                       };
                       context.WebScrappingSearches.Add(newSearch);
                   }
                   //else update?
               }

               context.SaveChanges();
           }
       }
    }
}