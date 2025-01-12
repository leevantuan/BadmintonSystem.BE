using BadmintonSystem.Contract.Constants;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;

internal sealed class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.ToTable(TableNames.AppUsers);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.IsDirector).HasDefaultValue(false);
        builder.Property(x => x.IsHeadOfDepartment).HasDefaultValue(false);
        builder.Property(x => x.ManagerId).HasDefaultValue(null);
        builder.Property(x => x.IsReceipient).HasDefaultValue(-1);
        builder.Property(x => x.AvatarUrl).HasDefaultValue(ImagesUrl.Avatar);

        // Each User can have many UserClaims
        builder.HasMany(e => e.Claims)
            .WithOne()
            .HasForeignKey(uc => uc.UserId)
            .IsRequired();

        // Each User can have many UserLogins
        builder.HasMany(e => e.Logins)
            .WithOne()
            .HasForeignKey(ul => ul.UserId)
            .IsRequired();

        // Each User can have many UserTokens
        builder.HasMany(e => e.Tokens)
            .WithOne()
            .HasForeignKey(ut => ut.UserId)
            .IsRequired();

        // Each User can have many entries in the UserRole join table
        builder.HasMany(e => e.UserRoles)
            .WithOne()
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();

        builder.HasMany(x => x.Notifications)
            .WithOne()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.PaymentMethods)
            .WithOne()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Reviews)
            .WithOne()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.UserAddresses)
            .WithOne()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasMany(x => x.Bookings)
            .WithOne()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // User - ChatRoom
        builder.HasOne(e => e.ChatRoom)
            .WithOne(c => c.User)
            .HasForeignKey<ChatRoom>(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
