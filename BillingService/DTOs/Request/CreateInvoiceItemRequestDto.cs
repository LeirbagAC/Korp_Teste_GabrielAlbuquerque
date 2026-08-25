using System.ComponentModel.DataAnnotations;

namespace BillingService.DTOs.Request;

public record CreateInvoiceItemRequestDto(
    [Required (ErrorMessage = "O Id do produto é obrigatório.")]
    int ProductId,
    
    [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser no mínimo 1.")]
    int Quantity
);