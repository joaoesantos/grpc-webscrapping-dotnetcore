using System.Threading.Tasks;
using ConsoleApp.WebScrapping.Implementation.PcDiga;

namespace ConsoleApp.WebScrapping.Definition
{
    public interface IScrapper
    {
        void Run();
        void OnComplete(IScrappingData scrappingData);
    }
}