using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;
internal class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable(nameof(TableNames.Address));

        builder.HasKey(x => x.Id);

        builder.Property(x => x.City).HasMaxLength(50).IsRequired();

        builder.HasMany(x => x.UserAddresses)
            .WithOne()
            .HasForeignKey(x => x.AddressId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
