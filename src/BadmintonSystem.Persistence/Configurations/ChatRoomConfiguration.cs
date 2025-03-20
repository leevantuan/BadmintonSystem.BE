using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;

internal sealed class ChatRoomConfiguration : IEntityTypeConfiguration<ChatRoom>
{
    public void Configure(EntityTypeBuilder<ChatRoom> builder)
    {
        builder.ToTable(TableNames.ChatRoom);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId).HasDefaultValue(null).IsRequired();

        //builder.HasMany(x => x.ChatMessages)
        //    .WithOne()
        //    .HasForeignKey(x => x.ChatRoomId)
        //    .OnDelete(DeleteBehavior.NoAction);
    }
}
