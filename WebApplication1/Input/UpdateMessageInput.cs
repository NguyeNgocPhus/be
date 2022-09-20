using WebApplication1.Entities;

namespace WebApplication1.Input;

public class UpdateMessageInput 
{
    public string Content { get; set; }
    public List<Guid> Liker { get; set; } = new List<Guid>();
  
}