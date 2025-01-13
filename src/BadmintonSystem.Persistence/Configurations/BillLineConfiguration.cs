using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;

internal sealed class BillLineConfiguration : IEntityTypeConfiguration<BillLine>
{
    public void Configure(EntityTypeBuilder<BillLine> builder)
    {
        builder.ToTable(TableNames.BillLine);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.BillId).HasDefaultValue(null).IsRequired();

        builder.Property(x => x.YardId).HasDefaultValue(null);

        builder.Property(x => x.StartTime).HasDefaultValue(null);

        builder.Property(x => x.EndTime).HasDefaultValue(null);

        builder.Property(x => x.TotalPrice).HasDefaultValue(null);
    }
}
