using InventoryService.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Data;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) {}

    public DbSet<Product> Products { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(builder =>
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.SequentialNumber)
                .ValueGeneratedOnAdd();
            
            builder.HasAlternateKey(p => p.SequentialNumber);
            
            builder.HasIndex(p => p.Code)
                .IsUnique(); 
            
            builder.Property(p => p.Code)
                .IsRequired()
                .HasMaxLength(50);
        });
    }
}