using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;

internal sealed class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable(TableNames.Booking);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.BookingDate).IsRequired();

        builder.Property(x => x.BookingTotal).IsRequired();

        builder.Property(x => x.OriginalPrice).IsRequired();

        builder.Property(x => x.BookingStatus).IsRequired();

        builder.Property(x => x.PaymentStatus).IsRequired();

        builder.Property(x => x.UserId).IsRequired();

        builder.Property(x => x.SaleId).HasDefaultValue(null);

        builder.Property(x => x.PercentPrePay).HasDefaultValue(null);

        builder.Property(x => x.FullName).HasDefaultValue(null);

        builder.Property(x => x.PhoneNumber).HasDefaultValue(null);

        builder.HasMany(x => x.BookingLines)
            .WithOne()
            .HasForeignKey(x => x.BookingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
