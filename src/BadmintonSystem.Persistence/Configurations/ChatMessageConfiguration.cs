using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BadmintonSystem.Persistence.Configurations;

internal sealed class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.ToTable(TableNames.ChatMessage);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ImageUrl).HasDefaultValue(null);

        builder.Property(x => x.Content).HasDefaultValue(null);

        builder.Property(x => x.IsAdmin).HasDefaultValue(null).IsRequired();

        builder.Property(x => x.IsRead).HasDefaultValue(null).IsRequired();

        builder.Property(x => x.ReadDate).HasDefaultValue(null).IsRequired();

        builder.Property(x => x.ChatRoomId).HasDefaultValue(null).IsRequired();
    }
}
