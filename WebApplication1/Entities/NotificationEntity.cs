namespace WebApplication1.Entities;

public class NotificationEntity
{
    public Guid Id { get; set; }
    public Guid UserTo { get; set; }
    public Guid UserFrom { get; set; }
    public string NotificationType { get; set; }
    public string Content { get; set; }
    public Guid RoomId { get; set; }
    public bool Opened { get; set; } = false;

}