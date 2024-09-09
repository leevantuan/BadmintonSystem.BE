using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Action = BadmintonSystem.Domain.Entities.Identity.Action;

namespace BadmintonSystem.Persistence.Configurations;

// IEntityTypeConfiguration<Action> == IEntityTypeConfiguration<T> used Model data == <T> == Action
// Cofigure for it
internal class ActionConfiguration : IEntityTypeConfiguration<Action>
{
    public void Configure(EntityTypeBuilder<Action> builder)
    {
        // Configuration here ==>
        builder.ToTable(TableNames.Actions);

        // Reset primary key == Id
        builder.HasKey(x => x.Id);

        // Config validator for Action
        builder.Property(x => x.Id).HasMaxLength(50); // Max length
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired(true); // Is Required and Length
        builder.Property(x => x.IsActive).HasDefaultValue(true); // If not declaration ==> Default = True
        builder.Property(x => x.SortOrder).HasDefaultValue(null); // If not declaration ==> Default = Null

        // Each User can have many Permission ==> Relationship
        // One - Many "User - Permission"
        //builder.HasMany(e => e.Permissions)
        //    .WithOne()
        //    .HasForeignKey(p => p.ActionId)
        //    .IsRequired();

        // Each User can have many ActionInFunction ==> Relationship
        // One - Many "User - ActionInFunction"
        builder.HasMany(e => e.ActionInFunctions)
            .WithOne()
            .HasForeignKey(aif => aif.ActionId)
            .IsRequired();
    }
}
