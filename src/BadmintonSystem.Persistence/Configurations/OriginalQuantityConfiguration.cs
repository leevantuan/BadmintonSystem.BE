using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;

internal sealed class OriginalQuantityConfiguration : IEntityTypeConfiguration<OriginalQuantity>
{
    public void Configure(EntityTypeBuilder<OriginalQuantity> builder)
    {
        builder.ToTable(TableNames.OriginalQuantity);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TotalQuantity).HasDefaultValue(null);

        builder.HasMany(x => x.Services)
            .WithOne()
            .HasForeignKey(x => x.OriginalQuantityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
