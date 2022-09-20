using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Input;

public class UpdateChatRoomInput
{
    [Required]
    public string Name { get; set; }
    public List<Guid> Users { get; set; }
}