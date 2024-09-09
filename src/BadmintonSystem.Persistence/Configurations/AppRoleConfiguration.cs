﻿using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;
internal class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
{
    public void Configure(EntityTypeBuilder<AppRole> builder)
    {
        builder.ToTable(TableNames.AppRoles);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Description)
            .HasMaxLength(250).IsRequired(true);
        builder.Property(x => x.RoleCode).HasMaxLength(50).IsRequired(true);

        // Each User can have many RoleClaims
        builder.HasMany(e => e.Claims)
            .WithOne()
            .HasForeignKey(uc => uc.RoleId)
            .IsRequired();

        // Each User can have many entries in the UserRole join table
        builder.HasMany(e => e.UserRoles)
            .WithOne()
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired();

        // Each User can have many ActionInFunction ==> Relationship
        // One - Many "User - ActionInFunction"
        builder.HasMany(e => e.PermissionInRoles)
            .WithOne()
            .HasForeignKey(aif => aif.RoleId)
            .IsRequired();
    }
}
