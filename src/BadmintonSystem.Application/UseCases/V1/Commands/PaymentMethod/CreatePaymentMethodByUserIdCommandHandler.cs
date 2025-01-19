using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.PaymentMethod;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Commands.PaymentMethod;

public sealed class CreatePaymentMethodByUserIdCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.PaymentMethod, Guid> paymentMethodRepository)
    : ICommandHandler<Command.CreatePaymentMethodByUserIdCommand>
{
    public async Task<Result> Handle
        (Command.CreatePaymentMethodByUserIdCommand request, CancellationToken cancellationToken)
    {
        _ = await context.AppUsers.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken)
            ?? throw new IdentityException.AppUserNotFoundException(request.UserId);

        Domain.Entities.PaymentMethod alreadyExist =
            await paymentMethodRepository.FindSingleAsync(x => x.Provider == request.Data.Provider, cancellationToken);

        if (alreadyExist != null)
        {
            throw new PaymentMethodException.PaymentMethodAlreadyExistException(request.Data.Provider);
        }

        Domain.Entities.PaymentMethod? paymentMethod = mapper.Map<Domain.Entities.PaymentMethod>(request.Data);

        paymentMethod.UserId = request.UserId;

        IQueryable<Domain.Entities.PaymentMethod> paymentMethods =
            paymentMethodRepository.FindAll(x => x.UserId == request.UserId);

        paymentMethod.IsDefault = DefaultEnum.TRUE;

        if (paymentMethods.Any())
        {
            paymentMethod.IsDefault = DefaultEnum.FALSE;
        }

        context.PaymentMethod.Add(paymentMethod);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
