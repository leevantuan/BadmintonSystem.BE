using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;

internal sealed class DayOffConfiguration : IEntityTypeConfiguration<DayOff>
{
    public void Configure(EntityTypeBuilder<DayOff> builder)
    {
        builder.ToTable(TableNames.DayOff);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Date).HasDefaultValue(null).IsRequired();

        builder.Property(x => x.Content).HasDefaultValue(null);
    }
}
