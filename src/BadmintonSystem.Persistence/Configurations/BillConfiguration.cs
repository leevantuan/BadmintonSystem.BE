using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;

internal sealed class BillConfiguration : IEntityTypeConfiguration<Bill>
{
    public void Configure(EntityTypeBuilder<Bill> builder)
    {
        builder.ToTable(TableNames.Bill);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TotalPrice).HasDefaultValue(null).IsRequired();

        builder.Property(x => x.TotalPayment).HasDefaultValue(null);

        builder.Property(x => x.Content).HasDefaultValue(null);

        builder.Property(x => x.Name).HasDefaultValue(null);

        builder.Property(x => x.UserId).HasDefaultValue(null);

        builder.Property(x => x.BookingId).HasDefaultValue(null);

        builder.Property(x => x.SaleId).HasDefaultValue(null);

        builder.HasMany(x => x.ServiceLines)
            .WithOne()
            .HasForeignKey(x => x.BillId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.BillLines)
            .WithOne()
            .HasForeignKey(x => x.BillId)
            .OnDelete(DeleteBehavior.Cascade);

        // builder.HasOne<Booking>()
        //     .WithOne()
        //     .HasForeignKey<Bill>(x => x.BookingId)
        //     .IsRequired(false)
        //     .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Booking)
            .WithOne()
            .HasForeignKey<Bill>(x => x.BookingId);
    }
}
