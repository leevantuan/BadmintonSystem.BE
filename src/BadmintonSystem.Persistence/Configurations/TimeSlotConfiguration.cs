using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;

internal sealed class TimeSlotConfiguration : IEntityTypeConfiguration<TimeSlot>
{
    public void Configure(EntityTypeBuilder<TimeSlot> builder)
    {
        builder.ToTable(TableNames.TimeSlot);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.StartTime).HasDefaultValue(null);

        builder.Property(x => x.EndTime).HasDefaultValue(null);

        builder.HasMany(x => x.BookingTimes)
            .WithOne()
            .HasForeignKey(x => x.TimeSlotId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.YardPrices)
            .WithOne()
            .HasForeignKey(x => x.TimeSlotId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.TimeSlotOfWeeks)
            .WithOne()
            .HasForeignKey(x => x.TimeSlotId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
