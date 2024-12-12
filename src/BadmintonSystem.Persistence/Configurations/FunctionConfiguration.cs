using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;

internal sealed class FunctionConfiguration : IEntityTypeConfiguration<Function>
{
    public void Configure(EntityTypeBuilder<Function> builder)
    {
        builder.ToTable(TableNames.Functions);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(200).IsRequired(true);
        builder.Property(x => x.ParentId).HasDefaultValue(null);

        builder.Property(x => x.CssClass).HasMaxLength(50).HasDefaultValue(null);
        builder.Property(x => x.Url).HasMaxLength(50).IsRequired(true);
        builder.Property(x => x.Status).HasDefaultValue(FunctionStatus.Active);
        builder.Property(x => x.SortOrder).HasDefaultValue(-1);
        builder.Property(x => x.Key).HasMaxLength(50).IsRequired(true);
        builder.Property(x => x.ActionValue).IsRequired(true);

        builder.Property(x => x.CreatedDate).IsRequired(true);
    }
}
