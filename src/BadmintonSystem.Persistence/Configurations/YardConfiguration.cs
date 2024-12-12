﻿using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;

internal sealed class YardConfiguration : IEntityTypeConfiguration<Yard>
{
    public void Configure(EntityTypeBuilder<Yard> builder)
    {
        builder.ToTable(TableNames.Yard);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasDefaultValue(null);

        builder.Property(x => x.YardTypeId).HasDefaultValue(null).IsRequired();

        builder.HasMany(x => x.BookingLines)
            .WithOne()
            .HasForeignKey(x => x.YardId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
