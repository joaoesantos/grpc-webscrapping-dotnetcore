using System.Reflection;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence.Contract;

namespace Persistence
{
    public class WebScrapperDbContext : DbContext, IWebScrapperDbContext
    {
        public DbSet<WebScrappingSearch> WebScrappingSearches { get; set; }
        public DbSet<WsRead> WsReads { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }

        public WebScrapperDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}