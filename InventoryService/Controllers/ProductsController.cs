using InventoryService.Models;
using InventoryService.Services;
using Microsoft.AspNetCore.Mvc;
using InventoryService.DTOs.Requests;

namespace InventoryService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var products = await productService.GetAllProductsAsync();
        return Ok(products); 
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProductById(int id)
    {
        var product = await productService.GetProductByIdAsync(id);
        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        var createdProduct = await productService.CreateAsync(product);
        return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
    }

	[HttpPost("decrease-stock")]
    public async Task<IActionResult> DecreaseStock([FromBody]List<DecreaseStockRequest> requests)
    {
        await productService.DecreaseStockAsync(requests);
        return NoContent();
    }

}