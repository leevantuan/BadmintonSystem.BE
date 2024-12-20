using System.Text;
using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.PaymentMethod;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Commands.PaymentMethod;

public sealed class UpdatePaymentMethodByUserIdCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.PaymentMethod, Guid> paymentMethodRepository)
    : ICommandHandler<Command.UpdatePaymentMethodByUserIdCommand>
{
    public async Task<Result> Handle
        (Command.UpdatePaymentMethodByUserIdCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.PaymentMethod paymentMethod =
            await paymentMethodRepository.FindByIdAsync(request.Data.Id, cancellationToken);

        paymentMethod.AccountNumber = request.Data.AccountNumber ?? paymentMethod.AccountNumber;
        paymentMethod.Expiry = request.Data.Expiry ?? paymentMethod.Expiry;
        paymentMethod.Provider = request.Data.Provider ?? paymentMethod.Provider;
        paymentMethod.UserId = request.Data.UserId ?? paymentMethod.UserId;

        if (request.Data.IsDefault == 1)
        {
            var updateQueryBuilder = new StringBuilder();
            updateQueryBuilder.Append(@$"UPDATE ""{nameof(Domain.Entities.PaymentMethod)}""
                                         SET ""{nameof(Domain.Entities.PaymentMethod.IsDefault)}"" = 0
                                         WHERE ""{nameof(Domain.Entities.PaymentMethod.UserId)}"" = '{request.UserId}' ");

            context.Database.ExecuteSqlRaw(updateQueryBuilder.ToString());

            await context.SaveChangesAsync(cancellationToken);
        }

        paymentMethod.IsDefault = (DefaultEnum)request.Data.IsDefault;

        return Result.Success();
    }
}
