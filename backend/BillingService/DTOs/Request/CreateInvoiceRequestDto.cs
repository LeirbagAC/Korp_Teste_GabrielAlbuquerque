using System.ComponentModel.DataAnnotations;

namespace BillingService.DTOs.Request;

public record CreateInvoiceRequestDto(
    [Required(ErrorMessage = "A nota fiscal deve conter itens.")]
    [MinLength(1, ErrorMessage = "A nota fiscal deve ter pelo menos um item.")]
    List<CreateInvoiceItemRequestDto> Items
);