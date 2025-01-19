using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.PaymentMethod;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.PaymentMethod;

public sealed class DeletePaymentMethodByUserIdCommandHandler(
    ApplicationDbContext context,
    IRepositoryBase<Domain.Entities.PaymentMethod, Guid> paymentMethodRepository)
    : ICommandHandler<Command.DeletePaymentMethodByUserIdCommand>
{
    public async Task<Result> Handle
        (Command.DeletePaymentMethodByUserIdCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.PaymentMethod paymentMethod =
            await paymentMethodRepository.FindByIdAsync(request.PaymentMethodId, cancellationToken);

        var paymentMethods = paymentMethodRepository.FindAll(x => x.UserId == request.UserId).ToList();

        if (paymentMethods.Count <= 1)
        {
            throw new ApplicationException("Payment method not deleted Because User just one Payment method.");
        }

        context.PaymentMethod.Remove(paymentMethod);

        if (paymentMethod.IsDefault == DefaultEnum.TRUE)
        {
            Domain.Entities.PaymentMethod? filteredList = paymentMethods.FirstOrDefault(x => x != paymentMethod);

            filteredList.IsDefault = DefaultEnum.TRUE;
        }

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
