namespace InventoryService.Models;

public class Product
{
    public int Id { get; set; }
    public string Code { get; set; } = "PROD-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
}