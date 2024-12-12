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

        builder.Property(x => x.Name).IsRequired(true);

        builder.Property(x => x.SellingPrice).IsRequired(true);

        builder.Property(x => x.PurchasePrice).IsRequired(true);

        builder.Property(x => x.CategoryId).IsRequired(true);

        builder.Property(x => x.ClubId).IsRequired(true);
    }
}
