namespace BadmintonSystem.Domain.Exceptions;
public class SaleException
{
    public class SaleNotFoundException : NotFoundException
    {
        public SaleNotFoundException(Guid saleId)
            : base($"===========> The Sale with the id {saleId} was not found.") { }
    }

    public class SaleBadRequestException : BadRequestException
    {
        public SaleBadRequestException(string message)
            : base($"===========> Bad Request: {message}.")
        {
        }
    }
}
