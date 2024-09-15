using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;

public class ClubConfiguration : IEntityTypeConfiguration<Club>
{
    public void Configure(EntityTypeBuilder<Club> builder)
    {
        builder.ToTable(TableNames.Club);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(126).IsRequired();

        builder.HasMany(x => x.Address)
            .WithOne()
            .HasForeignKey(x => x.ClubId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasMany(x => x.AdditionalServices)
            .WithOne()
            .HasForeignKey(x => x.ClubId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
