using BadmintonSystem.Application.UseCases.V1.Services;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Bill;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Bill;

public sealed class CloseBillCommandHandler(
    ApplicationDbContext context,
    IBillLineService billLineService,
    IBillService billService,
    IRepositoryBase<Domain.Entities.Bill, Guid> billRepository)
    : ICommandHandler<Command.CloseBillCommand>
{
    public async Task<Result> Handle(Command.CloseBillCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Bill billEntities = await billRepository.FindByIdAsync(request.BillId, cancellationToken)
                                            ?? throw new BillException.BillNotFoundException(request.BillId);

        List<BillLine> billLinesIsActive = await context.BillLine
            .Where(x => x.BillId == request.BillId && x.IsActive == ActiveEnum.IsActive).ToListAsync(cancellationToken);

        IEnumerable<Task> closeBillLineTasks =
            billLinesIsActive.Select(billLine => billLineService.CloseBillLineByBill(billLine.Id, cancellationToken));

        await Task.WhenAll(closeBillLineTasks);

        if (billEntities.BookingId != null)
        {
            await billService.ChangeYardActiveByBookingId(request.BillId, StatusEnum.TRUE, cancellationToken);
        }

        await billService.UpdateTotalPriceByBillId(request.BillId, cancellationToken);

        billEntities.Status = BillStatusEnum.CLOSE_BILL;

        return Result.Success();
    }
}
