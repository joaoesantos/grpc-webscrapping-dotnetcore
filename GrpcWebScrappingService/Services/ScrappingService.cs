using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcWebScrappingService.Persistence;
using GrpcWebScrappingService.StreamObserver.Implementation;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace GrpcWebScrappingService.Services
{
    public class ScrappingService : ScrappingServiceGrpc.ScrappingServiceGrpcBase
    {
        private ConcurrentDictionary<long, ScrappingDataStreamObserver> _listenersList;

        public ScrappingService()
        {
            _listenersList = new ConcurrentDictionary<long, ScrappingDataStreamObserver>();
        }
        
        public override Task ListScrappers(Empty request, IServerStreamWriter<Scrapper> responseStream, ServerCallContext context)
        {
            Console.WriteLine("A request");
            responseStream.WriteAsync(new Scrapper() {Id = 1, Name = "This is a test"});
            /*using (WebScrapperDbContext dbContext = new WebScrappingContextFactory().CreateDbContext(Array.Empty<string>()))
            {
                dbContext
                    .WebScrappingSearches
                    .ForEachAsync(search => responseStream
                        .WriteAsync(new Scrapper{Id = search.Id, Name = search.Url})
                    );
            }*/
            return Task.CompletedTask;
        }

        public override async Task<SubscribeResponse> SubscribeToScrapper(SubscribeScrapperRequest request, ServerCallContext context)
        {
            Subscription subscription = new Subscription();
            using (WebScrapperDbContext dbContext = new WebScrappingContextFactory().CreateDbContext(Array.Empty<string>()))
            {
                 Subscriber subscriber = dbContext
                    .Subscribers
                    .First(s => s.Username.Equals(request.Username));

                subscription = dbContext.Subscriptions.Add(new Subscription()
                {
                    SearchId = request.ScrapperId,
                    SubscriberId = subscriber.Id
                }).Entity;

                await dbContext.SaveChangesAsync();
            }
            
            return new SubscribeResponse
            {
                SubscriptionId = subscription.Id
            };
        }

        public override async Task ListenToSubscription(ListenRequest request, IServerStreamWriter<ListenResponse> responseStream, ServerCallContext context)
        {
            ScrappingDataStreamObserver streamObserver = _listenersList.
                GetOrAdd(request.SubscriptionId, _ => new ScrappingDataStreamObserver());
            using (WebScrapperDbContext dbContext = new WebScrappingContextFactory().CreateDbContext(Array.Empty<string>()))
            {
                var query = dbContext.Subscriptions
                    .Join(
                        dbContext.WsReads,
                        subscription => subscription.SearchId,
                        read => read.SearchId,
                        (subscription, read) => new
                        {
                            SubscriptionId = subscription.Id,
                            ReadId = read.Id,
                            Data = read.Data,
                            Name = read.Name
                        })
                    .OrderByDescending(o => o.ReadId)
                    .First(o => o.SubscriptionId == request.SubscriptionId);

                if (query.ReadId > request.LastMessageId)
                {
                    await responseStream.WriteAsync(new ListenResponse
                    {
                        Data = query.Data,
                        MessageId = query.ReadId,
                        Name = query.Name
                    });
                }
            }

            await streamObserver.OnListener(responseStream, request.SubscriptionId);
            return;
        }

        public override async Task<CreateAccountResponse> CreateAccount(CreateAccountRequest request, ServerCallContext context)
        {
            CreateAccountResponse response = new CreateAccountResponse();
            using (WebScrapperDbContext dbContext = new WebScrappingContextFactory().CreateDbContext(Array.Empty<string>()))
            {
                var entity = dbContext.Subscribers.Add(new Subscriber
                {
                    Username = request.Username
                });

                await dbContext.SaveChangesAsync();
                response.UserId = entity.Entity.Id;
            }

            return response;
        }
    }
}