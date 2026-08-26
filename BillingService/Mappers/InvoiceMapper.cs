using BillingService.DTOs.Response;
using BillingService.Models;

namespace BillingService.Mappers;

using Riok.Mapperly.Abstractions;

[Mapper(IgnoreObsoleteMembersStrategy = IgnoreObsoleteMembersStrategy.Both)]
public partial class InvoiceMapper
{
    [MapProperty(nameof(Invoice.Id), "InvoiceId")]
    public partial IQueryable<InvoiceResponseDto> ProjectToDto(IQueryable<Invoice> query);
    
    [MapProperty(nameof(Invoice.Id), "InvoiceId")]
    public partial InvoiceResponseDto MapToDto(Invoice invoice);
}