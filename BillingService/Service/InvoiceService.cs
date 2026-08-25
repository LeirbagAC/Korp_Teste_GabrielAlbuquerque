using BillingService.Data;
using BillingService.Clients;
using BillingService.Models;
using BillingService.DTOs.Request;
using BillingService.DTOs.Response;
using BillingService.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BillingService.Service;

public class InvoiceService(BillingDbContext context, IInventoryClient inventoryClient) : IInvoiceService
{
    public async Task<InvoiceResponseDto> CreateInvoiceAsync(CreateInvoiceRequestDto request)
    {
        var invoice = new Invoice
        {
            Status = InvoiceStatus.Aberta,
            Items = new List<InvoiceItem>()
        };

        //   Refatorar para não fazer um loop diretamente no banco
        foreach (var itemRequest in request.Items)
        {
            var product = await inventoryClient.GetProductByIdAsync(itemRequest.ProductId);

            var invoiceItem = new InvoiceItem
            {
                ProductId = product.Id,
                ProductCode = product.Code,
                ProductDescription = product.ProductName,
                Quantity = itemRequest.Quantity
            };

            invoice.Items.Add(invoiceItem);
        }

        context.Invoices.Add(invoice);
        await context.SaveChangesAsync();

        return new InvoiceResponseDto(
            invoice.Id,
            invoice.SequentialNumber,
            invoice.Items.Select(i => new InvoiceItemResponseDto(
                i.ProductId,
                i.ProductCode,
                i.ProductDescription,
                i.Quantity
            )).ToList(),
            invoice.Status.ToString(),
            invoice.CreatedAt
        );
    }
    
    public async Task PrintInvoiceAsync(int invoiceId)
    {
        var invoice = await context.Invoices
            .Include(i => i.Items) 
            .FirstOrDefaultAsync(i => i.Id == invoiceId);

        if (invoice is null)
            throw new NotFoundException($"Nota Fiscal {invoiceId} não encontrada.");

        if (invoice.Status != InvoiceStatus.Aberta)
            throw new DomainException("Apenas notas com status 'Aberta' podem ser impressas.");

        var stockRequests = invoice.Items.Select(item => new DecreaseStockRequestDto(
            ProductId: item.ProductId,
            Quantity: item.Quantity
        )).ToList();

        await inventoryClient.DecreaseStockAsync(stockRequests);

        invoice.Status = InvoiceStatus.Fechada;
        await context.SaveChangesAsync();
    }
}