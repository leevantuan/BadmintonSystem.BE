using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;

internal sealed class YardPriceConfiguration : IEntityTypeConfiguration<YardPrice>
{
    public void Configure(EntityTypeBuilder<YardPrice> builder)
    {
        builder.ToTable(TableNames.YardPrice);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.YardId).HasDefaultValue(null).IsRequired();

        builder.Property(x => x.TimeSlotId).HasDefaultValue(null).IsRequired();

        builder.Property(x => x.PriceId).HasDefaultValue(null);

        builder.Property(x => x.EffectiveDate).HasDefaultValue(null).IsRequired();

        builder.Property(x => x.IsBooking).HasDefaultValue(null).IsRequired();

        builder.HasMany(x => x.BookingLines)
            .WithOne()
            .HasForeignKey(x => x.YardPriceId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
