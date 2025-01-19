using BadmintonSystem.Application.UseCases.V1.Services;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Bill;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Bill;

public sealed class OpenYardByBillCommandHandler(
    IBillLineService billLineService,
    IRepositoryBase<Domain.Entities.Bill, Guid> billRepository)
    : ICommandHandler<Command.OpenYardByBillCommand>
{
    public async Task<Result> Handle(Command.OpenYardByBillCommand request, CancellationToken cancellationToken)
    {
        _ = await billRepository.FindByIdAsync(request.Data.BillId, cancellationToken)
            ?? throw new BillException.BillNotFoundException(request.Data.BillId);

        await billLineService.OpenBillLineByBill(request.Data.YardId.Value, request.Data.BillId,
            cancellationToken);

        return Result.Success();
    }
}
