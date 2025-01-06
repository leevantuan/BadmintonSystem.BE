using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;

internal sealed class ComboFixedConfiguration : IEntityTypeConfiguration<ComboFixed>
{
    public void Configure(EntityTypeBuilder<ComboFixed> builder)
    {
        builder.ToTable(TableNames.ComboFixed);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Price).HasDefaultValue(null).IsRequired();

        builder.Property(x => x.Content).HasDefaultValue(null);

        builder.HasMany(x => x.ServiceLines)
            .WithOne()
            .HasForeignKey(x => x.ComboFixedId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
