using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Contract
{
    public interface IWebScrapperDbContext
    {
        DbSet<WebScrappingSearch> WebScrappingSearches { get; set; }
        DbSet<WsRead> WsReads { get; set; }
        
        DbSet<Subscription> Subscriptions { get; set; }
        DbSet<Subscriber> Subscribers { get; set; }
    }
}