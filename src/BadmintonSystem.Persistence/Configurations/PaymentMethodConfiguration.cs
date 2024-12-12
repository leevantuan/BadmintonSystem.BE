using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;

internal sealed class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.ToTable(TableNames.PaymentMethod);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.AccountNumber).HasDefaultValue(null);
        builder.Property(x => x.Expiry).HasDefaultValue(null);
        builder.Property(x => x.Provider).HasDefaultValue(null);
        builder.Property(x => x.IsDefault).HasDefaultValue(null);
        //builder.Property(x => x.PaymentTypeId).HasDefaultValue(null);
        builder.Property(x => x.UserId).HasDefaultValue(null);
    }
}
