using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsoleApp.Persistence.EntityConfiguration
{
    public class SubscriberConfiguration : IEntityTypeConfiguration<Subscriber>
    {
        public void Configure(EntityTypeBuilder<Subscriber> builder)
        {
            builder.HasKey(subscriber => subscriber.Id);
            builder.Property(subscriber => subscriber.Username).IsRequired();
            builder.HasMany(subscriber => subscriber.Subscriptions)
                .WithOne(subscription => subscription.Subscriber)
                .HasForeignKey(subscription => subscription.SubscriberId);

            builder.HasIndex(subscriber => subscriber.Username).IsUnique();
        }
    }
}