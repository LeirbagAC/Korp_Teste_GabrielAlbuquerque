namespace BillingService.DTOs.Response;

public record ProductResponseDto(
    int Id,
    string Code,
    string ProductName,
    int Quantity
);