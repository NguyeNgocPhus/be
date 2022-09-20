using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Entities;

namespace WebApplication1.Database.Configuration;

public class MessageConfiguration: IEntityTypeConfiguration<MessageEntity>
{
    public void Configure(EntityTypeBuilder<MessageEntity> builder)
    {
        builder.ToTable("Messages");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Content).IsRequired();
        builder.Property(x => x.Sender).IsUnicode().IsRequired().HasColumnType("uuid");
        builder.Property(x => x.Liker).IsUnicode().IsRequired().HasColumnType("jsonb");
        builder.Property(x => x.Readers).IsUnicode().IsRequired().HasColumnType("jsonb");
        builder.Property(x => x.ChatRoom).IsUnicode().IsRequired().HasColumnType("uuid");
    }
}