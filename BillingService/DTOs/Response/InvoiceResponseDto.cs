namespace BillingService.DTOs.Response;

public record InvoiceResponseDto(
    int Id,
    string SequentialNumber,
    List<InvoiceItemResponseDto> Items,
    string Status, 
    DateTime CreatedAt
);