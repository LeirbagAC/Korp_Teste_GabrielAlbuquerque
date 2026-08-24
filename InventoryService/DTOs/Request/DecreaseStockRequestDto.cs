using System.ComponentModel.DataAnnotations;
    
namespace InventoryService.DTOs.Request;

public record DecreaseStockRequestDto(
    [Required(ErrorMessage = "O ID do produto é obrigatório.")]
    int ProductId, 
    
    [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
    int Quantity 
);