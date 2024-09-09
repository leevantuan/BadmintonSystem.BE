using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;
internal class FunctionConfiguration : IEntityTypeConfiguration<Function>
{
    public void Configure(EntityTypeBuilder<Function> builder)
    {
        // Configuration here ==>
        builder.ToTable(TableNames.Functions);

        // Reset primary key == Id
        builder.HasKey(x => x.Id);

        // Config validator for Action
        builder.Property(x => x.Id).HasMaxLength(50);
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired(true);
        builder.Property(x => x.ParrentId)
            .HasMaxLength(50)
            .HasDefaultValue(null);
        builder.Property(x => x.CssClass).HasMaxLength(50).HasDefaultValue(null);
        builder.Property(x => x.Url).HasMaxLength(50).IsRequired(true);
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(x => x.SortOrder).HasDefaultValue(null);

        // Each User can have many Permission
        //builder.HasMany(e => e.Permissions)
        //    .WithOne()
        //    .HasForeignKey(p => p.FunctionId)
        //    .IsRequired();

        // Each User can have many ActionInFunction
        builder.HasMany(e => e.ActionInFunctions)
            .WithOne()
            .HasForeignKey(aif => aif.FunctionId)
            .IsRequired();

        // Each User can have many PermissionInRole
        builder.HasMany(e => e.PermissionInRoles)
            .WithOne()
            .HasForeignKey(aif => aif.FunctionId)
            .IsRequired();

        // Each User can have many PermissionInUser
        builder.HasMany(e => e.PermissionInUsers)
            .WithOne()
            .HasForeignKey(aif => aif.FunctionId)
            .IsRequired();
    }
}
