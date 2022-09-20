namespace WebApplication1.Entities;

public class MessageEntity
{
    public Guid Id { get; set; }
    public Guid Sender { get; set; }
    public bool Starred { get; set; } = false;
    public string Content { get; set; }
    public List<UserEntity> Readers { get; set; } = new List<UserEntity>();
    public Guid ChatRoom { get; set; }
    public List<UserEntity> Liker { get; set; } = new List<UserEntity>();
    public Guid ReplyFrom { get; set; }
    public DateTimeOffset CreateTime { get; set; }
    public string? FileAttach { get; set; }
}
