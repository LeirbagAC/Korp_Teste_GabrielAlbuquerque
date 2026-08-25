using BillingService.DTOs.Request;
using BillingService.DTOs.Response;

namespace BillingService.Service;

public interface IInvoiceService
{
    Task PrintInvoiceAsync(int invoiceId);
    Task<InvoiceResponseDto> CreateInvoiceAsync(CreateInvoiceRequestDto request);
}