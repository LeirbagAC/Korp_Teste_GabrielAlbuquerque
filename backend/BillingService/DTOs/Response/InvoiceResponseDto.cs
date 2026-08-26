namespace BillingService.DTOs.Response;

public record InvoiceResponseDto(
    string SequentialNumber,
    List<InvoiceItemResponseDto> Items,
    string Status, 
    DateTime CreatedAt
);