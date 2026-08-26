using System.ComponentModel.DataAnnotations;
    
namespace InventoryService.DTOs.Request;

public record DecreaseStockRequestDto(
    [Required(ErrorMessage = "O código do produto é obrigatório.")]
    string ProductCode, 
    
    [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
    int Quantity 
);