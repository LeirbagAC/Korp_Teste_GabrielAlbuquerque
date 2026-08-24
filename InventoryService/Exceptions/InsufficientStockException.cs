namespace InventoryService.Exceptions;

public class InsufficientStockException : DomainException
{
    public InsufficientStockException(string productName, int curreStock, int requested)
        : base($"Estoque insuficiente para o produto '{productName}'. Estoque atual: {curreStock}, quantidade solicitada: {requested}.")
    {
    }
}