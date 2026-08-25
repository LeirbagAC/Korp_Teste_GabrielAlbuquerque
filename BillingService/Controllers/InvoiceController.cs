using BillingService.DTOs.Request;
using BillingService.DTOs.Response;
using BillingService.Service;
using Microsoft.AspNetCore.Mvc;

namespace BillingService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvoicesController(IInvoiceService invoiceService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<InvoiceResponseDto>> CreateInvoiceAsync(CreateInvoiceRequestDto request)
    {
        var createdInvoice = await invoiceService.CreateInvoiceAsync(request);
        return StatusCode(StatusCodes.Status201Created, createdInvoice); 
    } 
    
    [HttpPost("{id}/print")]
    public async Task<IActionResult> PrintInvoice(int id)
    {
        await invoiceService.PrintInvoiceAsync(id);
        return NoContent(); 
    }
}