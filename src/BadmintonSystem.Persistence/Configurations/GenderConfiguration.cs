using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;
internal sealed class GenderConfiguration : IEntityTypeConfiguration<Gender>
{
    public void Configure(EntityTypeBuilder<Gender> builder)
    {
        builder.ToTable(TableNames.Gender);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasDefaultValue(null);

        builder.HasMany(x => x.Users)
            .WithOne()
            .HasForeignKey(x => x.GenderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
