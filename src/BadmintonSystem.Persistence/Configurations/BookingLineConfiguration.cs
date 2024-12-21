using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;

internal sealed class BookingLineConfiguration : IEntityTypeConfiguration<BookingLine>
{
    public void Configure(EntityTypeBuilder<BookingLine> builder)
    {
        builder.ToTable(TableNames.BookingLine);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TotalPrice).HasDefaultValue(null);

        builder.Property(x => x.YardPriceId).HasDefaultValue(null);

        builder.Property(x => x.BookingId).HasDefaultValue(null);

        builder.HasMany(x => x.BookingTimes)
            .WithOne()
            .HasForeignKey(x => x.BookingLineId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
