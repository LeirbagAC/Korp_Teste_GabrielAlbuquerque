using InventoryService.Exceptions;

namespace InventoryService.Models;

public class Product
{
    public Guid Id { get; private set; }
    public int SequentialNumber { get; private set; }
    public string Code { get; private set; } 
    public string Name { get; private set; }
    public int Quantity { get; private set; }

    protected Product() { }

    public Product(string name, int quantity)
    {
        Id = Guid.NewGuid();
        Code = Id.ToString(); 
        Name = name;
        Quantity = quantity;
    }

    public void GenerateCode()
    {
        Code = $"PROD-{SequentialNumber:D4}";
    }

    public void DecreaseStock(int amountToDecrease)
    {
        if (Quantity < amountToDecrease)
            throw new InsufficientStockException(Name, Quantity, amountToDecrease);
            
        Quantity -= amountToDecrease;
    }
}