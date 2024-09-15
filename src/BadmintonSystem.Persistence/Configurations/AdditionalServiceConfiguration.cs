using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;
internal class AdditionalServiceConfiguration : IEntityTypeConfiguration<AdditionalService>
{
    public void Configure(EntityTypeBuilder<AdditionalService> builder)
    {
        builder.ToTable(TableNames.AdditionalService);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Price).IsRequired();
    }
}
