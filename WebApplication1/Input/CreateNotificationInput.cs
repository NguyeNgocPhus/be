namespace WebApplication1.Input;

public class CreateNotificationInput
{
    public Guid UserTo { get; set; }
    public string NotificationType { get; set; }
    public string Content { get; set; }
    public Guid RoomId { get; set; }
}