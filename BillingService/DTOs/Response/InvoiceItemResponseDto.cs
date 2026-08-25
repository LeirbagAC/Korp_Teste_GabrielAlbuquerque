namespace BillingService.DTOs.Response;

public record InvoiceItemResponseDto(
    int ProductId,
    string ProductCode,
    string ProductDescription,
    int Quantity
);