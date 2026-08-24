using InventoryService.DTOs.Request;
using InventoryService.DTOs.Response;

namespace InventoryService.Services;

public interface IProductService
{
    Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync();
    Task<ProductResponseDto> GetProductByIdAsync(int id);
    Task<ProductResponseDto> CreateAsync(ProductRequestDto product);
    Task DecreaseStockAsync(List<DecreaseStockRequestDto> requests);
}