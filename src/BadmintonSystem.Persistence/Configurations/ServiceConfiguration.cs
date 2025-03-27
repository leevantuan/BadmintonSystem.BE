using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;

internal sealed class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.ToTable(TableNames.Service);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired();

        builder.Property(x => x.SellingPrice).HasDefaultValue(null);

        builder.Property(x => x.Unit).HasDefaultValue(null);

        builder.Property(x => x.PurchasePrice).HasDefaultValue(null);

        builder.Property(x => x.Description).HasDefaultValue(null);

        builder.Property(x => x.ImageLink).HasDefaultValue(null);

        builder.Property(x => x.QuantityInStock).HasDefaultValue(null);

        builder.Property(x => x.CategoryId).HasDefaultValue(null);

        builder.Property(x => x.OriginalQuantityId).HasDefaultValue(null);

        builder.HasMany(x => x.ServiceLines)
            .WithOne()
            .HasForeignKey(x => x.ServiceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.InventoryReceipts)
            .WithOne()
            .HasForeignKey(x => x.ServiceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
