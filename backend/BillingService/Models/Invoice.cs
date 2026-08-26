namespace BillingService.Models;

public class Invoice
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public int SequentialNumber { get; set; }

    public InvoiceStatus Status { get; set; } = InvoiceStatus.Aberta;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public List<InvoiceItem> Items { get; set; } = new();
}