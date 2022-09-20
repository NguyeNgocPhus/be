namespace WebApplication1.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
    public string? Avatar { get; set; } = String.Empty;

    public ICollection<ProductEntity> Products = new List<ProductEntity>();
}