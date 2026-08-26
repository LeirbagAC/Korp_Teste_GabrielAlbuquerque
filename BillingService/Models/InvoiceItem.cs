namespace BillingService.Models;

public class InvoiceItem
{
    public int Id { get; set; }
    
    public Guid InvoiceId { get; set; }
    
    public Invoice Invoice { get; set; } = null!;
    
    public string ProductCode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    
    public int Quantity { get; set; }
}