using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;

internal sealed class BookingTimeConfiguration : IEntityTypeConfiguration<BookingTime>
{
    public void Configure(EntityTypeBuilder<BookingTime> builder)
    {
        builder.ToTable(TableNames.BookingTime);

        builder.HasKey(x => new { x.BookingLineId, x.TimeSlotId });

        builder.Property(x => x.TimeSlotId).HasDefaultValue(null);

        builder.Property(x => x.BookingLineId).HasDefaultValue(null);
    }
}
