using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;

internal sealed class ReviewImageConfiguration : IEntityTypeConfiguration<ReviewImage>
{
    public void Configure(EntityTypeBuilder<ReviewImage> builder)
    {
        builder.ToTable(TableNames.ReviewImage);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ImageLink).HasDefaultValue(null).IsRequired();

        builder.Property(x => x.ReviewId).HasDefaultValue(null).IsRequired();
    }
}
