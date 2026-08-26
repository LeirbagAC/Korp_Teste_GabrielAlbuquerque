namespace BillingService.DTOs.Response;

public record InvoiceResponseDto(
    int InvoiceId,
    string SequentialNumber,
    List<InvoiceItemResponseDto> Items,
    string Status, 
    DateTime CreatedAt
);