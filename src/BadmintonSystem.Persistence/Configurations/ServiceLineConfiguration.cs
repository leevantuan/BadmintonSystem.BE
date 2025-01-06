using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;

internal sealed class ServiceLineConfiguration : IEntityTypeConfiguration<ServiceLine>
{
    public void Configure(EntityTypeBuilder<ServiceLine> builder)
    {
        builder.ToTable(TableNames.ServiceLine);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ServiceId).HasDefaultValue(null);

        builder.Property(x => x.ComboFixedId).HasDefaultValue(null);

        builder.Property(x => x.BillId).HasDefaultValue(null);

        builder.Property(x => x.Quantity).HasDefaultValue(null);
    }
}
