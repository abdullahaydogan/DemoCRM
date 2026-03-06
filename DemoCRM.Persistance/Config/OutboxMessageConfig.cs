using DemoCRM.Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoCRM.Persistance.Config
{
    public class OutboxMessageConfig : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.ToTable("OutboxMessages");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.EventType).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Payload).IsRequired().HasColumnType("text");
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.ProcessedDate).IsRequired(false);
            builder.Property(x => x.IsProcessed).IsRequired().HasDefaultValue(false);
            builder.Property(x => x.RetryCount).IsRequired().HasDefaultValue(0);
            builder.Property(x => x.LastError).HasColumnType("text");
            builder.HasIndex(x => new { x.IsProcessed, x.CreatedDate });
        }
    }
}
