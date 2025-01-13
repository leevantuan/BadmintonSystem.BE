using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;

internal sealed class InventoryReceiptConfiguration : IEntityTypeConfiguration<InventoryReceipt>
{
    public void Configure(EntityTypeBuilder<InventoryReceipt> builder)
    {
        builder.ToTable(TableNames.InventoryReceipt);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Price).HasDefaultValue(null).IsRequired();

        builder.Property(x => x.Quantity).HasDefaultValue(null);

        builder.Property(x => x.Unit).HasDefaultValue(null);

        builder.Property(x => x.ServiceId).HasDefaultValue(null);

        builder.Property(x => x.ProviderId).HasDefaultValue(null);
    }
}
