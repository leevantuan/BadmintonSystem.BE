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

        builder.HasKey(x => new { x.BillId, x.YardPriceId });
    }
}
