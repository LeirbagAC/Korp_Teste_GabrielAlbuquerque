namespace BillingService.DTOs.Response;

public record InvoiceItemResponseDto(
    string ProductCode,
    int Quantity
);