using InventoryService.DTOs.Request;
using InventoryService.DTOs.Response;
using InventoryService.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetProducts()
    {
        var productsDto = await productService.GetAllProductsAsync();
        return Ok(productsDto); 
    }

    [HttpGet("{productCode}")]
    public async Task<ActionResult<ProductResponseDto>> GetProductByCode(string productCode)
    {
        var productDto = await productService.GetProductByCodeAsync(productCode);
        return Ok(productDto);
    }

    [HttpPost]
    public async Task<ActionResult<ProductResponseDto>> CreateProduct(ProductRequestDto request)
    {
        var createdProduct = await productService.CreateAsync(request);
        
        return CreatedAtAction(
            nameof(GetProductByCode),
            new { productCode = createdProduct.Code }, 
            createdProduct
        );
    }

    [HttpPost("decrease-stock")]
    public async Task<IActionResult> DecreaseStock([FromBody]List<DecreaseStockRequestDto> requests)
    {
        await productService.DecreaseStockAsync(requests);
        return NoContent();
    }
}