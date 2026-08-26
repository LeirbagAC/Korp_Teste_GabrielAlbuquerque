namespace InventoryService.DTOs.Response;

public record ProductResponseDto
(   
    string Code,
    string ProductName,
    int Quantity
);