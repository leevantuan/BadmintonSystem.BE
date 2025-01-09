using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;

internal sealed class PriceConfiguration : IEntityTypeConfiguration<Price>
{
    public void Configure(EntityTypeBuilder<Price> builder)
    {
        builder.ToTable(TableNames.Price);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.YardPrice).HasDefaultValue(null).IsRequired();

        builder.Property(x => x.IsDefault).IsRequired();

        builder.Property(x => x.Detail).HasDefaultValue(null);

        builder.Property(x => x.StartTime).HasDefaultValue(null);

        builder.Property(x => x.EndTime).HasDefaultValue(null);

        builder.Property(x => x.DayOfWeek).HasDefaultValue(null);

        builder.Property(x => x.YardTypeId).HasDefaultValue(null);

        builder.HasMany(x => x.YardPrices)
            .WithOne()
            .HasForeignKey(x => x.PriceId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
