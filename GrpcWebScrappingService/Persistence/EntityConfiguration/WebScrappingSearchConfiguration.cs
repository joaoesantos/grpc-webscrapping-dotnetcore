using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsoleApp.Persistence.EntityConfiguration
{
    public class WebScrappingSearchConfiguration : IEntityTypeConfiguration<WebScrappingSearch>
    {
        public void Configure(EntityTypeBuilder<WebScrappingSearch> builder)
        {
            builder.HasKey(search => search.Id);
            builder.Property(search => search.Url)
                .IsRequired();

            builder.HasMany(search => search.Reads)
                .WithOne(read => read.WebScrappingSearch)
                .HasForeignKey(read => read.SearchId);

            builder.HasMany(search => search.Subscriptions)
                .WithOne(subscription => subscription.SubscribedSearch)
                .HasForeignKey(subscription => subscription.SearchId);

        }
    }
}