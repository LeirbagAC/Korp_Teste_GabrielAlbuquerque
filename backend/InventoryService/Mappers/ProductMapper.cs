using InventoryService.DTOs.Response;
using InventoryService.Models;
using Riok.Mapperly.Abstractions;

namespace InventoryService.Mappers;

[Mapper]
public partial class ProductMapper
{
    [MapProperty(nameof(Product.Name), "ProductName")]
    public partial ProductResponseDto MapToProductResponseDto(Product product);

    [MapProperty(nameof(Product.Name), "ProductName")]
    public partial IQueryable<ProductResponseDto> ProjectToDto(IQueryable<Product> query);

}