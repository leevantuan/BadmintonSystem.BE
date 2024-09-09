using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;
internal class PermissionInUserConfiguration : IEntityTypeConfiguration<PermissionInUser>
{
    public void Configure(EntityTypeBuilder<PermissionInUser> builder)
    {
        builder.ToTable(TableNames.PermissionInUsers);

        // Relationship Many to many Action - Function
        builder.HasKey(x => new { x.UserId, x.FunctionId });
    }
}
