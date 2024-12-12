using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;

internal sealed class ClubImageConfiguration : IEntityTypeConfiguration<ClubImage>
{
    public void Configure(EntityTypeBuilder<ClubImage> builder)
    {
        builder.ToTable(TableNames.ClubImage);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ImageLink).HasDefaultValue(null).IsRequired();

        builder.Property(x => x.ClubId).HasDefaultValue(null).IsRequired();
    }
}
