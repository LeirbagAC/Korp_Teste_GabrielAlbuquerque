using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var brasiliaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
        
        var problemDetails = new ProblemDetails
        {
            Instance = httpContext.Request.Path 
        };

        if (exception is NotFoundException notFoundException)
        {
            problemDetails.Status = StatusCodes.Status404NotFound;
            problemDetails.Title = "Recurso Não Encontrado";
            problemDetails.Detail = notFoundException.Message;
        }
        else if (exception is DomainException domainException)
        {
            problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
            problemDetails.Title = "Violação de Regra de Negócio";
            problemDetails.Detail = domainException.Message;

            problemDetails.Extensions["horaDoErro"] =
                TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, brasiliaTimeZone);
        }
        else
        {
            return false;
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}