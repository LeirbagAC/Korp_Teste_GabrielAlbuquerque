using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BillingService.Exceptions;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not NotFoundException and not DomainException)
        {
            logger.LogError(exception, "Erro inesperado ocorrido: {Message}", exception.Message);
        }
        else
        {
            logger.LogWarning("Exceção de negócio/cliente lançada: {Message}", exception.Message);
        }

        var (statusCode, title, detail) = MapException(exception);

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path
        };

        if (exception is DomainException)
        {
            var brasiliaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
            problemDetails.Extensions["horaDoErro"] = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, brasiliaTimeZone);
        }

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true; 
    }

    private static (int StatusCode, string Title, string Detail) MapException(Exception exception)
    {
        return exception switch
        {
            NotFoundException => (
                StatusCodes.Status404NotFound, 
                "Recurso Não Encontrado", 
                exception.Message),
                
            DomainException => (
                StatusCodes.Status422UnprocessableEntity, 
                "Violação de Regra de Negócio", 
                exception.Message),
                
            _ => (
                StatusCodes.Status500InternalServerError, 
                "Erro Interno no Servidor", 
                "Ocorreu um erro inesperado no sistema. Tente novamente mais tarde.")
        };
    }
}