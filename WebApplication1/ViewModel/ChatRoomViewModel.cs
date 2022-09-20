using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Entities;

namespace WebApplication1.ViewModel;


[Keyless]
[Table("ChatRoomViewModel")]
public class ChatRoomViewModel
{
    [Column("Id")]
    public Guid Id { get; set; }
    [Column("UserId")]
    public Guid UserId { get; set; }
    [Column("Name")]
    public string Name { get; set; }
    [Column("UserName")]
    public string UserName { get; set; }
    [Column("Email")]
    public string Email { get; set; }
    [Column("LatestMessage")]
    public string? LatestMessage { get; set; } = String.Empty;
    [Column("Members", TypeName = "jsonb")]
    public List<UserViewModel> Members { get; set; } = new List<UserViewModel>();
}

public class UserViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } 
    public string Email { get; set; }
    public string Password { get; set; }
}