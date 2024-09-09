using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;
internal class PermissionInRoleConfiguration : IEntityTypeConfiguration<PermissionInRole>
{
    public void Configure(EntityTypeBuilder<PermissionInRole> builder)
    {
        builder.ToTable(TableNames.PermissionInRoles);

        // Relationship Many to many Action - Function
        builder.HasKey(x => new { x.RoleId, x.FunctionId });
    }
}
