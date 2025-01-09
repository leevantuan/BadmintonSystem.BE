using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;

internal sealed class FixedScheduleConfiguration : IEntityTypeConfiguration<FixedSchedule>
{
    public void Configure(EntityTypeBuilder<FixedSchedule> builder)
    {
        builder.ToTable(TableNames.FixedSchedule);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId).HasDefaultValue(null);

        builder.Property(x => x.PhoneNumber).HasDefaultValue(null);

        builder.Property(x => x.StartDate).HasDefaultValue(null).IsRequired();

        builder.Property(x => x.EndDate).HasDefaultValue(null).IsRequired();

        builder.Property(x => x.YardId).HasDefaultValue(null).IsRequired();

        builder.HasMany(x => x.DaysOfWeeks)
            .WithOne()
            .HasForeignKey(x => x.FixedScheduleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
