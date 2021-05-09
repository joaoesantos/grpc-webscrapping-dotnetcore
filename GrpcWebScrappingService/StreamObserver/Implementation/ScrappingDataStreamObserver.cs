using System.Collections.Concurrent;
using System.Threading.Tasks;
using ConsoleApp.WebScrapping.Definition;
using Grpc.Core;
using GrpcWebScrappingService.StreamObserver.Definition;

namespace GrpcWebScrappingService.StreamObserver.Implementation
{
    public class ScrappingDataStreamObserver : IStreamObserver<IScrappingData>
    {
        private readonly ConcurrentDictionary<long, IServerStreamWriter<ListenResponse>> _listeners;

        public ScrappingDataStreamObserver()
        {
            _listeners = new ConcurrentDictionary<long, IServerStreamWriter<ListenResponse>>();
        }

        public Task OnListener(IServerStreamWriter<ListenResponse> streamWriter, long listenerId)
        {
            _listeners.AddOrUpdate(listenerId, _ => streamWriter, (_, _) => streamWriter );
            return Task.CompletedTask;
        }

        public Task Publish(IScrappingData data)
        {
            
            Parallel.ForEach(_listeners.Values, async writer =>
            {
                await writer.WriteAsync(new ListenResponse
                {
                    Data = data.GetData(),
                    //TODO Review what i want todo with this field
                    MessageId = data.GetId(),
                    Name = data.GetName()
                });
            });

            return Task.CompletedTask;
        }
    }
}