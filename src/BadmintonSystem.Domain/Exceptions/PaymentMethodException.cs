namespace BadmintonSystem.Domain.Exceptions;
public static class PaymentMethodException
{
    public class PaymentMethodNotFoundException : NotFoundException
    {
        public PaymentMethodNotFoundException(Guid paymentMethodId)
            : base($"The paymentMethod with the id {paymentMethodId} was not found.")
        { }
    }
}
