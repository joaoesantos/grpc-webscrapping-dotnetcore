using System;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsoleApp.Persistence.EntityConfiguration
{
    public class WsReadConfiguration : IEntityTypeConfiguration<WsRead>
    {
        public void Configure(EntityTypeBuilder<WsRead> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Data)
                .IsRequired();
            builder.Property(r => r.ReadDateTime)
                .HasDefaultValue(DateTime.Now);
            builder.Property(r => r.Name)
                .IsRequired();
            builder.HasIndex(r => r.Name).IsUnique();
        }
    }
}