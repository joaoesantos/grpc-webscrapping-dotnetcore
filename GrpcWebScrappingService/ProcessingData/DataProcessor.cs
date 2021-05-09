using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using ConsoleApp.WebScrapping.Definition;
using Domain;
using GrpcWebScrappingService.Persistence;
using Persistence;

namespace ConsoleApp.ProcessingData
{
    public class DataProcessor
    {
        private readonly ConcurrentQueue<IScrappingData> _queue;
        private readonly ConcurrentDictionary<IScrappingDataIdentifier, int> _previousReadings;
        
        public DataProcessor(ConcurrentQueue<IScrappingData> queue, ConcurrentDictionary<IScrappingDataIdentifier, int> previousReadings)
        {
            _queue = queue;
            _previousReadings = previousReadings;
        }

        /// <summary>
        /// Method responsible to add data to a fifo
        /// </summary>
        /// <param name="data"></param>
        public Task PublishData(IScrappingData data)
        {
            return Task.Run(() => _queue.Enqueue(data));
        }

        public void Process()
        {
            //there is something to process
            if (!_queue.IsEmpty)
            {
                //get data to process
                bool isSuccess = _queue.TryDequeue(out var data);
                if (isSuccess)
                {
                    //verifies if call to database is necessary
                    int hash;
                    bool exists = _previousReadings.TryGetValue(data.GetIdentifier(), out hash);
                    bool addToDb = false;
                    if (exists)
                    {
                        if (!hash.Equals(data.GetHash()))
                        {
                            //adds read to database
                            bool success;
                            do
                            {
                                success = _previousReadings
                                              .TryGetValue(data.GetIdentifier(), out var previousHash) &&
                                          _previousReadings.TryUpdate(
                                              data.GetIdentifier(), data.GetHash(), previousHash);
                            } while (!success);

                            addToDb = true;
                        }
                        
                    }
                    else
                    {
                        bool success;
                        do
                        {
                            success = _previousReadings.TryAdd(data.GetIdentifier(), data.GetHash());
                        } while (!success);

                        addToDb = true;
                    }

                    if (addToDb)
                    {
                        //TODO have using context around all logic so that you only save at the end
                        using (WebScrapperDbContext context = new WebScrappingContextFactory().CreateDbContext(Array.Empty<string>()))
                        {
                            context.WsReads.Add(new WsRead()
                            {
                                ReadDateTime = DateTime.Now,
                                //TODO Review IScrappingData getters to have SearchId
                                SearchId =  data.GetId(),
                                Data = data.GetData(),
                                Name = data.GetName()
                            });
                            context.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}