using InventoryService.Data;
using InventoryService.Models;
using Microsoft.EntityFrameworkCore;
using InventoryService.DTOs.Requests;


namespace InventoryService.Services;

public class ProductService(InventoryDbContext context) : IProductService
{
    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await context.Products
		.AsNoTracking()
		.ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
		var product = await context.Products.FindAsync(id);
		
		if(product is null) throw new KeyNotFoundException($"Produto com ID {id} não encontrado.");

        return product;
    }

	
    public async Task<Product> CreateAsync(Product product)
    {
        context.Products.Add(product);
        await context.SaveChangesAsync();
        return product;
    }

	public async Task DecreaseStockAsync(List<DecreaseStockRequest> requests)
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