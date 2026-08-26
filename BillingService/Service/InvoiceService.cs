using BillingService.Data;
using BillingService.Clients;
using BillingService.Models;
using BillingService.DTOs.Request;
using BillingService.DTOs.Response;
using BillingService.Exceptions;
using BillingService.Mappers;
using Microsoft.EntityFrameworkCore;

namespace BillingService.Service;

public class InvoiceService(BillingDbContext context, IInventoryClient inventoryClient, InvoiceMapper invoiceMapper) : IInvoiceService
{
    public async Task<IEnumerable<InvoiceResponseDto>> GetInvoicesAsync()
    {
        return await invoiceMapper.ProjectToDto(context.Invoices.AsNoTracking())
            .ToListAsync();
    }
    
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
                ProductName = product.ProductName,
                Quantity = itemRequest.Quantity
            };

            invoice.Items.Add(invoiceItem);
        }

        context.Invoices.Add(invoice);
        await context.SaveChangesAsync();

        return invoiceMapper.MapToDto(invoice);
    }
    
    public async Task PrintInvoiceAsync(int sequentialNumber)
    {
        var invoice = await context.Invoices
            .Include(i => i.Items) 
            .FirstOrDefaultAsync(i => i.SequentialNumber == sequentialNumber);

        if (invoice is null)
            throw new NotFoundException($"Nota Fiscal {sequentialNumber} não encontrada.");

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