using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Entities;

namespace WebApplication1.Database.Configuration;

public class NotificationConfiguration :  IEntityTypeConfiguration<NotificationEntity>
{
    public void Configure(EntityTypeBuilder<NotificationEntity> builder)
    {
        builder.ToTable("Notification");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Content).IsRequired().HasColumnType("text");;
        builder.Property(x => x.NotificationType).IsUnicode().IsRequired().HasColumnType("text");
        builder.Property(x => x.UserFrom).IsUnicode().IsRequired().HasColumnType("uuid");
        builder.Property(x => x.UserTo).IsUnicode().IsRequired().HasColumnType("uuid");
        builder.Property(x => x.Opened).IsUnicode().IsRequired().HasColumnType("bool");
        builder.Property(x => x.RoomId).IsUnicode().IsRequired().HasColumnType("uuid");

    }
}