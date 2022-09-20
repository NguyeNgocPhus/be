namespace WebApplication1.Entities;

public class ChatRoomEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } 
    public string? LatestMessage { get; set; } = String.Empty;
    public List<UserEntity> Users { get; set; } = new List<UserEntity>();
}