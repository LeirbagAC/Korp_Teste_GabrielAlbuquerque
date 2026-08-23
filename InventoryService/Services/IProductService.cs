using InventoryService.Models;
using InventoryService.DTOs.Requests;

namespace InventoryService.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);
    Task<Product> CreateAsync(Product product);
    Task DecreaseStockAsync(List<DecreaseStockRequest> requests);
}