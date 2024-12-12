using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;

internal sealed class ClubInformationConfiguration : IEntityTypeConfiguration<ClubInformation>
{
    public void Configure(EntityTypeBuilder<ClubInformation> builder)
    {
        builder.ToTable(TableNames.ClubInformation);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FacebookPageLink).HasDefaultValue(null);

        builder.Property(x => x.InstagramLink).HasDefaultValue(null);

        builder.Property(x => x.MapLink).HasDefaultValue(null);

        builder.Property(x => x.ClubId).HasDefaultValue(null).IsRequired();
    }
}
