using InventoryService.Data;
using InventoryService.DTOs.Request;
using InventoryService.DTOs.Response;
using InventoryService.Exceptions;
using InventoryService.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Services;

public class ProductService(InventoryDbContext context) : IProductService
{
    public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync()
    {
        return await context.Products
		.AsNoTracking()
		.Select(product => new ProductResponseDto(
			product.Code,
			product.Name,
			product.Quantity
		))
		.ToListAsync();
    }

    public async Task<ProductResponseDto> GetProductByCodeAsync(string productCode)
    {
	    var product = await context.Products.FirstOrDefaultAsync(p => p.Code == productCode);
        
	    if (product is null) 
		    throw new NotFoundException($"Produto com código {productCode} não encontrado.");

	    return new ProductResponseDto(
		    product.Code,
		    product.Name,
		    product.Quantity
	    );    
    }
    
    public async Task<ProductResponseDto> CreateAsync(ProductRequestDto request)
    {
	    var product = new Product(request.ProductName, request.Quantity);
        
	    var transaction = await context.Database.BeginTransactionAsync();
        
	    try
	    {
		    context.Products.Add(product);
		    await context.SaveChangesAsync();
            
		    product.GenerateCode();
		    await context.SaveChangesAsync();
            
		    await transaction.CommitAsync();
            
		    return new ProductResponseDto(
			    product.Code,
			    product.Name,
			    product.Quantity
		    );
	    }
	    catch
	    {
		    await transaction.RollbackAsync();
		    throw;
	    }
    }

    public async Task DecreaseStockAsync(List<DecreaseStockRequestDto> requests)
    {    
	    var productCodes = requests.Select(r => r.ProductCode).ToList();

	    var products = await context.Products
		    .Where(p => productCodes.Contains(p.Code))
		    .ToListAsync();

	    foreach (var request in requests)
	    {
		    var product = products.FirstOrDefault(p => p.Code == request.ProductCode);

		    if (product is null) 
			    throw new NotFoundException($"Produto com código {request.ProductCode} não encontrado.");

		    product.DecreaseStock(request.Quantity);
	    }

	    await context.SaveChangesAsync();
    }
}