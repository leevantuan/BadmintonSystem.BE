using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;

internal sealed class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable(TableNames.Sale);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(100).HasDefaultValue(null).IsRequired();

        builder.Property(x => x.Percent).HasDefaultValue(null).IsRequired();

        builder.Property(x => x.StartDate).HasDefaultValue(null).IsRequired();

        builder.Property(x => x.EndDate).HasDefaultValue(null).IsRequired();

        builder.Property(x => x.IsActive).HasDefaultValue(null).IsRequired();

        builder.HasMany(x => x.Bookings)
            .WithOne()
            .HasForeignKey(x => x.SaleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
