using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DayOfWeek = BadmintonSystem.Domain.Entities.DayOfWeek;

namespace BadmintonSystem.Persistence.Configurations;

internal sealed class DayOfWeekConfiguration : IEntityTypeConfiguration<DayOfWeek>
{
    public void Configure(EntityTypeBuilder<DayOfWeek> builder)
    {
        builder.ToTable(TableNames.DayOfWeek);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FixedScheduleId).HasDefaultValue(null).IsRequired();

        builder.Property(x => x.WeekName).HasDefaultValue(null).IsRequired();

        builder.HasMany(x => x.TimeSlotOfWeeks)
            .WithOne()
            .HasForeignKey(x => x.DayOfWeekId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
