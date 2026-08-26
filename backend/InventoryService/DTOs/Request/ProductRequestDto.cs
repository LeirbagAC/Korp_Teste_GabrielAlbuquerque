using System.ComponentModel.DataAnnotations;

namespace InventoryService.DTOs.Request;

public record ProductRequestDto
(
    [Required(ErrorMessage = "O nome do produto é obrigatório.")]
    string ProductName,
    
    [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
    int Quantity
);