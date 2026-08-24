using InventoryService.Data;
using InventoryService.DTOs.Request;
using InventoryService.DTOs.Response;
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
			product.Id,
			product.Code,
			product.Name,
			product.Quantity
		))
		.ToListAsync();
    }

    public async Task<ProductResponseDto> GetProductByIdAsync(int id)
    {
		var product = await context.Products.FindAsync(id);
		
		if(product is null) throw new KeyNotFoundException($"Produto com ID {id} não encontrado.");

		return new ProductResponseDto(
			product.Id,
			product.Code,
			product.Name,
			product.Quantity
		);    
    }
    
    public async Task<ProductResponseDto> CreateAsync(ProductRequestDto request)
    {
	    var product = new Product()
	    {
		    Name = request.ProductName,
		    Quantity = request.Quantity
	    };
	    
        context.Products.Add(product);
        await context.SaveChangesAsync();
        
        return new ProductResponseDto(
	        product.Id,
	        product.Code,
	        product.Name,
	        product.Quantity
        );
    }

	public async Task DecreaseStockAsync(List<DecreaseStockRequestDto> requests)
	{	
    	var productIds = requests.Select(r => r.ProductId).ToList();

    	var products = await context.Products
        	.Where(p => productIds.Contains(p.Id))
        	.ToListAsync();

    	foreach (var request in requests)
    	{
        	var product = products.FirstOrDefault(p => p.Id == request.ProductId);

        	if (product is null) throw new KeyNotFoundException($"Produto com ID {request.ProductId} não encontrado.");

		    if (product.Quantity < request.Quantity) throw new InvalidOperationException($"Estoque insuficiente para o produto {product.Name}. Estoque atual: {product.Quantity}, quantidade solicitada: {request.Quantity}.");

        	product.Quantity -= request.Quantity;
    	}

    	await context.SaveChangesAsync();
	}

}