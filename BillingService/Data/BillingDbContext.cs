using BillingService.Models;
using Microsoft.EntityFrameworkCore;

namespace BillingService.Data;

public class BillingDbContext : DbContext
{
    public BillingDbContext(DbContextOptions<BillingDbContext> options) : base(options) { }

    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceItem> InvoiceItems => Set<InvoiceItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Invoice>(builder =>
        {
            builder.HasKey(i => i.Id);
            
            builder.HasAlternateKey(i => i.SequentialNumber);

            builder.Property(i => i.SequentialNumber)
                .ValueGeneratedOnAdd();
               
            builder.HasMany(i => i.Items)
                .WithOne(item => item.Invoice)
                .HasForeignKey(item => item.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<InvoiceItem>(builder =>
        {
            builder.HasKey(i => i.Id);
        });

        base.OnModelCreating(modelBuilder);
    }
}