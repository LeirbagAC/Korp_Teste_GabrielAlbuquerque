using System.Net;
using System.Net.Http.Json;
using BillingService.DTOs.Request;
using BillingService.DTOs.Response;
using BillingService.Exceptions; 

namespace BillingService.Clients;

public class InventoryClient(HttpClient httpClient) : IInventoryClient
{
    public async Task DecreaseStockAsync(List<DecreaseStockRequestDto> requests)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("/api/products/decrease-stock", requests);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new DomainException($"Falha ao baixar estoque: {errorMessage}");
            }
        }
        catch (HttpRequestException)
        {
            throw new DomainException("Não foi possível se comunicar com o Estoque. O serviço pode estar fora do ar no momento.");
        }
    }
    
    public async Task<ProductResponseDto> GetProductByCodeAsync(string productCode)
    {
        try
        {
            var response = await httpClient.GetAsync($"/api/products/{productCode}");

            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new NotFoundException($"Produto com ID {productCode} não foi encontrado no catálogo.");
            
            if (!response.IsSuccessStatusCode)
                throw new DomainException("Serviço de estoque indisponível no momento.");

            return await response.Content.ReadFromJsonAsync<ProductResponseDto>()
                   ?? throw new DomainException("Erro ao ler dados do produto.");

        }
        catch (HttpRequestException)
        {
            throw new DomainException("Não foi possível se comunicar com o Estoque. O serviço pode estar fora do ar no momento.");
        }
    }
    
}