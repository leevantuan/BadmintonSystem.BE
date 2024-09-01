using BadmintonSystem.Persistence.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// Purpose => Generate Table and reset key == Guid
namespace BadmintonSystem.Persistence.Configurations;

// Internal just used in Assembly here
// Sealed can't Inheritance == "Kế thừa"
// Override Authoziration of IdentityFramework
// IEntityTypeConfiguration => this is configuration for Entities
// Purpose => It will configuration for OnModelCreating in DbContext, Instead of writing in Func DbContext
internal sealed class AppUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<Guid>>
{
    // Configure IdentityUserRole == Override it
    public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
    {
        // ToTable => Override table name == TableNames.AppUserRoles
        builder.ToTable(TableNames.AppUserRoles);

        // Generate primary key contain: RoleId and UserId
        // new { a, b } => generate multiple primary keys
        builder.HasKey(x => new { x.RoleId, x.UserId });
    }
}

internal sealed class AppRoleClaimConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<Guid>> builder)
    {
        builder.ToTable(TableNames.AppRoleClaims);

        builder.HasKey(x => x.RoleId);
    }
}

internal sealed class AppUserClaimConfiguration : IEntityTypeConfiguration<IdentityUserClaim<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserClaim<Guid>> builder)
    {
        builder.ToTable(TableNames.AppUserClaims);

        builder.HasKey(x => x.UserId);
    }
}

internal sealed class AppUserLoginConfiguration : IEntityTypeConfiguration<IdentityUserLogin<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserLogin<Guid>> builder)
    {
        builder.ToTable(TableNames.AppUserLogins);

        builder.HasKey(x => x.UserId);
    }
}

internal sealed class AppUserTokenConfiguration : IEntityTypeConfiguration<IdentityUserToken<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserToken<Guid>> builder)
    {
        builder.ToTable(TableNames.AppUserTokens);

        builder.HasKey(x => x.UserId);
    }
}
