using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Action = BadmintonSystem.Domain.Entities.Identity.Action;

namespace BadmintonSystem.Persistence.Configurations;
internal sealed class ActionConfiguration : IEntityTypeConfiguration<Action>
{
    public void Configure(EntityTypeBuilder<Action> builder)
    {
        builder.ToTable(TableNames.Actions);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(200).IsRequired(true);
        builder.Property(x => x.SortOrder).HasDefaultValue(null);
        builder.Property(x => x.IsActive).HasDefaultValue(true);
    }
}
