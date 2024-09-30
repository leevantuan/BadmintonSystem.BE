using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;
public class SaleConfiguration : IEntityTypeConfiguration<Domain.Entities.Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable(nameof(TableNames.Sale));

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
    }
}
