using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;

internal sealed class YardTypeConfiguration : IEntityTypeConfiguration<YardType>
{
    public void Configure(EntityTypeBuilder<YardType> builder)
    {
        builder.ToTable(TableNames.YardType);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasDefaultValue(null);

        builder.Property(x => x.Price).HasDefaultValue(null);

        builder.HasMany(x => x.Yards)
            .WithOne()
            .HasForeignKey(x => x.YardTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
