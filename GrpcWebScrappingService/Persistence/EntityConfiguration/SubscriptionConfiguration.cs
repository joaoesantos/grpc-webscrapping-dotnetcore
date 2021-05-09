using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsoleApp.Persistence.EntityConfiguration
{
    public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.HasKey(subscription => subscription.Id);
            builder.HasOne(subscription => subscription.LastRead)
                .WithMany()
                .HasForeignKey(subscription => subscription.LastReadId);
        }
    }
}