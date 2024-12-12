using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;

internal sealed class PaymentTypeConfiguration : IEntityTypeConfiguration<PaymentType>
{
    public void Configure(EntityTypeBuilder<PaymentType> builder)
    {
        builder.ToTable(TableNames.PaymentType);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasDefaultValue(null);

        // builder.HasMany(x => x.PaymentMethods)
        //     .WithOne()
        //     .HasForeignKey(x => x.PaymentTypeId)
        //     .OnDelete(DeleteBehavior.Cascade);
    }
}
