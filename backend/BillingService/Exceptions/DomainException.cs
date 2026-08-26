namespace BillingService.Exceptions;

public class DomainException : AppException
{
    public DomainException(string message) : base(message, StatusCodes.Status422UnprocessableEntity) { }
}