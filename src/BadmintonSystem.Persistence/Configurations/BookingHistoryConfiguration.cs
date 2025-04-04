using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;

public class BookingHistoryConfiguration : IEntityTypeConfiguration<BookingHistory>
{
    public void Configure(EntityTypeBuilder<BookingHistory> builder)
    {
        builder.ToTable(TableNames.BookingHistory);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.BookingId).HasDefaultValue(null);

        builder.Property(x => x.UserId).HasDefaultValue(null);

        builder.Property(x => x.ClubName).HasDefaultValue(null);

        builder.Property(x => x.StartTime).HasDefaultValue(null);

        builder.Property(x => x.PlayDate).HasDefaultValue(null);

        builder.Property(x => x.CreatedDate).HasDefaultValue(null);

        builder.Property(x => x.PaymentStatus).HasDefaultValue(null);

        builder.Property(x => x.TotalPrice).HasDefaultValue(null);

        builder.Property(x => x.TenantCode).HasDefaultValue(null);
    }
}
