using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Entities;

namespace WebApplication1.Database.Configuration;

public class FileUpdateConfiguration: IEntityTypeConfiguration<FileUpdateReadModel>
{
    public void Configure(EntityTypeBuilder<FileUpdateReadModel> builder)
    {
        // Define keys + index
        builder.HasKey(u => new {u.Id});
        builder.Property(u => u.Id).ValueGeneratedOnAdd();
        builder.Property(u => u.Code).ValueGeneratedOnAdd();

        builder.HasIndex(u => u.Name).IsUnique();
        builder.Property(t=>t.Data).IsUnicode().IsRequired().HasColumnType("bytea");
        builder.ToTable("Files","FileStorage");
    }
}