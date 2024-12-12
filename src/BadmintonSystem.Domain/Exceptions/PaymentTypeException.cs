namespace BadmintonSystem.Domain.Exceptions;
public static class PaymentTypeException
{
    public class PaymentTypeNotFoundException : NotFoundException
    {
        public PaymentTypeNotFoundException(Guid paymentTypeId)
            : base($"The paymentType with the id {paymentTypeId} was not found.")
        { }
    }
}
