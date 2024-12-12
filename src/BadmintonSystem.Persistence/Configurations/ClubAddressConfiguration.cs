using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;
internal sealed class ClubAddressConfiguration : IEntityTypeConfiguration<ClubAddress>
{
    public void Configure(EntityTypeBuilder<ClubAddress> builder)
    {
        builder.ToTable(TableNames.ClubAddress);

        builder.HasKey(x => new { x.AddressId, x.ClubId });

        builder.Property(x => x.Branch).HasDefaultValue(null);
    }
}
