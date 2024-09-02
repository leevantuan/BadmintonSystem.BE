using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;
internal class GenderConfiguration : IEntityTypeConfiguration<Gender>
{
    public void Configure(EntityTypeBuilder<Gender> builder)
    {
        builder.ToTable(TableNames.Gender);

        // Reset primary key == Id
        builder.HasKey(x => x.Id);

        // Config validator for Action
        builder.Property(x => x.Name).HasMaxLength(50).IsRequired(); // Max length and required
    }
}
