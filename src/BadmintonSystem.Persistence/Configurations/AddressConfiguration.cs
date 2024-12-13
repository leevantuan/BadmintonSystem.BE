using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;

internal sealed class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable(TableNames.Address);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Unit).HasDefaultValue(null);
        builder.Property(x => x.Street).HasDefaultValue(null);
        builder.Property(x => x.AddressLine1).HasDefaultValue(null);
        builder.Property(x => x.AddressLine2).HasDefaultValue(null);
        builder.Property(x => x.City).HasDefaultValue(null);
        builder.Property(x => x.Province).HasDefaultValue(null);

        builder.HasMany(x => x.ClubAddresses)
            .WithOne()
            .HasForeignKey(x => x.AddressId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
