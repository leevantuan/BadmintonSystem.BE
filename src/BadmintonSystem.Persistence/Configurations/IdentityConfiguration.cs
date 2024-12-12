using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Persistence.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;

internal sealed class AppUserRoleConfiguration : IEntityTypeConfiguration<AppUserRole>
{
    public void Configure(EntityTypeBuilder<AppUserRole> builder)
    {
        builder.ToTable(TableNames.AppUserRoles);

        builder.HasKey(x => new { x.RoleId, x.UserId });

        builder.Property(x => x.IsDefault).HasDefaultValue(false);
    }
}

internal sealed class AppRoleClaimConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<Guid>> builder)
    {
        builder.ToTable(TableNames.AppRoleClaims);

        builder.HasKey(x => x.Id);
    }
}

internal sealed class AppUserClaimConfiguration : IEntityTypeConfiguration<IdentityUserClaim<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserClaim<Guid>> builder)
    {
        builder.ToTable(TableNames.AppUserClaims);

        builder.HasKey(x => x.Id);
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
