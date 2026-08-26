using BillingService.DTOs.Request;
using BillingService.DTOs.Response;

namespace BillingService.Clients;

public interface IInventoryClient
{
    Task DecreaseStockAsync(List<DecreaseStockRequestDto> requests);
    Task<ProductResponseDto> GetProductByCodeAsync(string productCode);

}