using System.Threading.Tasks;
using Grpc.Core;

namespace GrpcWebScrappingService.StreamObserver.Definition
{
    public interface IStreamObserver<T>
    {
        Task OnListener(IServerStreamWriter<ListenResponse> streamWriter, long listenerId);
        Task Publish(T data);
    }
}