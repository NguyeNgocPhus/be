namespace WebApplication1.Entities;

public class ProductEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public int Quantity { get; set; } = 0;
    
    public UserEntity? User { get; set; }
    public Guid UserId { get; set; }
}