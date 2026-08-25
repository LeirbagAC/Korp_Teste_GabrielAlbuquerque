namespace BillingService.Models;

public class Invoice
{
    public int Id { get; set; }
    
    public string SequentialNumber { get; set; } = string.Empty;
    
    public InvoiceStatus Status { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public List<InvoiceItem> Items { get; set; } = new();
}