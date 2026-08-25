using System.Net;
using System.Net.Http.Json;
using BillingService.DTOs.Request;
using BillingService.DTOs.Response;
using BillingService.Exceptions; 

namespace BillingService.Clients;

public class InventoryClient(HttpClient httpClient) : IInventoryClient
{
    // private IInventoryClient _inventoryClientImplementation;

    public async Task DecreaseStockAsync(List<DecreaseStockRequestDto> requests)
    {
        var response = await httpClient.PostAsJsonAsync("/api/products/decrease-stock", requests);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new DomainException($"Falha ao baixar estoque: {errorMessage}");
        }
    }
    
    public async Task<ProductResponseDto> GetProductByIdAsync(int id)
    {
        var response = await httpClient.GetAsync($"/api/products/{id}");

        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new NotFoundException($"Produto com ID {id} não foi encontrado no catálogo.");

            throw new DomainException("Serviço de estoque indisponível no momento.");
        }

        return await response.Content.ReadFromJsonAsync<ProductResponseDto>() 
               ?? throw new DomainException("Erro ao processar os dados do produto.");
    }
    
}