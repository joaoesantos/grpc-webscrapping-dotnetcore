using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Persistence;

namespace GrpcWebScrappingService.Persistence
{
    public class WebScrappingContextFactory: IDesignTimeDbContextFactory<WebScrapperDbContext>
    {
        public WebScrapperDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();
            DatabaseConfiguration databaseConfiguration= configuration
                .GetSection("DatabaseConnection").Get<DatabaseConfiguration>();

            var builder = new DbContextOptionsBuilder<WebScrapperDbContext>();
            builder.UseNpgsql($"host={databaseConfiguration.Host};port={databaseConfiguration.Port};database={databaseConfiguration.Database};user id={databaseConfiguration.User};password={databaseConfiguration.Password};");
            return new WebScrapperDbContext(builder.Options);
        }
    }
}