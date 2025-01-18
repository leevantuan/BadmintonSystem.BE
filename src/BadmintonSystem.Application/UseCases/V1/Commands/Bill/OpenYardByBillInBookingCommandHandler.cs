using AutoMapper;
using BadmintonSystem.Application.UseCases.V1.Services;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Bill;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Bill;

public sealed class OpenYardByBillInBookingCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IBillService billService,
    IRepositoryBase<Domain.Entities.Bill, Guid> billRepository)
    : ICommandHandler<Command.OpenYardByBillInBookingCommand>
{
    public async Task<Result> Handle
        (Command.OpenYardByBillInBookingCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Bill billEntities = await billRepository.FindByIdAsync(request.BillId, cancellationToken)
                                            ?? throw new BillException.BillNotFoundException(request.BillId);

        if (billEntities.Status == BillStatusEnum.ACTIVE_NOW || billEntities.Status == BillStatusEnum.CLOSE_BILL)
        {
            throw new ApplicationException("You cannot open yard because it is not booking.");
        }

        await billService.ChangeYardActiveByBookingId(request.BillId, StatusEnum.FALSE, cancellationToken);

        billEntities.Status = BillStatusEnum.ACTIVE_NOW;

        return Result.Success();
    }
}
