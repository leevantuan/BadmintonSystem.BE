using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;

internal sealed class TimeSlotOfWeekConfiguration : IEntityTypeConfiguration<TimeSlotOfWeek>
{
    public void Configure(EntityTypeBuilder<TimeSlotOfWeek> builder)
    {
        builder.ToTable(TableNames.TimeSlotOfWeek);

        builder.HasKey(x => new { x.DayOfWeekId, x.TimeSlotId });

        builder.Property(x => x.TimeSlotId).HasDefaultValue(null);

        builder.Property(x => x.DayOfWeekId).HasDefaultValue(null);
    }
}
