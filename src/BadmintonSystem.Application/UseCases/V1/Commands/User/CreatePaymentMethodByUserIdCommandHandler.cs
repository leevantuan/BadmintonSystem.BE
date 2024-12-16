using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.User;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.User;

public sealed class CreatePaymentMethodByUserIdCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<PaymentMethod, Guid> paymentMethodRepository)
    : ICommandHandler<Command.CreatePaymentMethodByUserIdCommand>
{
    public async Task<Result> Handle
        (Command.CreatePaymentMethodByUserIdCommand request, CancellationToken cancellationToken)
    {
        _ = context.AppUsers.FirstOrDefault(x => x.Id == request.UserId)
            ?? throw new IdentityException.AppUserNotFoundException(request.UserId);

        PaymentMethod alreadyExist =
            await paymentMethodRepository.FindSingleAsync(x => x.Provider == request.Data.Provider, cancellationToken);

        if (alreadyExist != null)
        {
            throw new PaymentMethodException.PaymentMethodAlreadyExistException(request.Data.Provider);
        }

        PaymentMethod? paymentMethod = mapper.Map<PaymentMethod>(request.Data);

        paymentMethod.UserId = request.UserId;
        paymentMethod.IsDefault = (DefaultEnum)request.Data.IsDefault;

        context.PaymentMethod.Add(paymentMethod);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
