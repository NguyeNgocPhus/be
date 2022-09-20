using Identity.Configuration;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Database.Configuration;
using WebApplication1.Entities;
using WebApplication1.ViewModel;

namespace WebApplication1.Database;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<UserEntity> Users { get; set; }
    public DbSet<MessageEntity> Messages { get; set; }
    public DbSet<ChatRoomEntity> Chats { get; set; }
    public DbSet<NotificationEntity> Notification { get; set; }
    public DbSet<ChatRoomViewModel> ChatViewModels { get; set; }
    public DbSet<FileUpdateReadModel> FileUpdates { get; set; }
    public DbSet<ProductEntity> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new MessageConfiguration());
        modelBuilder.ApplyConfiguration(new ChatRoomConfiguration());
        modelBuilder.ApplyConfiguration(new NotificationConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }
}