using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;

internal sealed class ClubConfiguration : IEntityTypeConfiguration<Club>
{
    public void Configure(EntityTypeBuilder<Club> builder)
    {
        builder.ToTable(TableNames.Club);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Code).HasDefaultValue(null);
        builder.Property(x => x.Hotline).HasDefaultValue(null);
        builder.Property(x => x.OpeningTime).IsRequired();
        builder.Property(x => x.ClosingTime).IsRequired();

        builder.HasMany(e => e.ClubAddresses)
            .WithOne()
            .HasForeignKey(x => x.ClubId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasMany(e => e.ClubImages)
            .WithOne()
            .HasForeignKey(x => x.ClubId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(e => e.ClubInformation)
            .WithOne()
            .HasForeignKey<ClubInformation>(e => e.ClubId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
