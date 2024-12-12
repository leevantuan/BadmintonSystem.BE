using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;

internal sealed class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable(TableNames.Review);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Comment).HasDefaultValue(null).IsRequired();

        builder.Property(x => x.RatingValue).HasDefaultValue(null).IsRequired();

        builder.HasMany(x => x.ReviewImages)
            .WithOne()
            .HasForeignKey(x => x.ReviewId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
