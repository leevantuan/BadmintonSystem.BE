using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;

internal sealed class ProviderConfiguration : IEntityTypeConfiguration<Provider>
{
    public void Configure(EntityTypeBuilder<Provider> builder)
    {
        builder.ToTable(TableNames.Provider);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasDefaultValue(null).IsRequired();

        builder.Property(x => x.PhoneNumber).HasDefaultValue(null);

        builder.Property(x => x.Address).HasDefaultValue(null);

        builder.HasMany(x => x.InventoryReceipts)
            .WithOne()
            .HasForeignKey(x => x.ProviderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
