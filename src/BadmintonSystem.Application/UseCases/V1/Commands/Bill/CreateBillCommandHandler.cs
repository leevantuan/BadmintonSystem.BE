using AutoMapper;
using BadmintonSystem.Application.UseCases.V1.Services;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Bill;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Bill;

public sealed class CreateBillCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IBillLineService billLineService,
    IRepositoryBase<Domain.Entities.Bill, Guid> billRepository)
    : ICommandHandler<Command.CreateBillCommand>
{
    public async Task<Result> Handle(Command.CreateBillCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Bill? billEntities = mapper.Map<Domain.Entities.Bill>(request);

        billEntities.Status = BillStatusEnum.ACTIVE_NOW;
        billEntities.BookingId = null;

        billRepository.Add(billEntities);
        await context.SaveChangesAsync(cancellationToken);

        await billLineService.OpenBillLineByBill(request.Data.YardId ?? Guid.Empty, billEntities.Id, cancellationToken);

        return Result.Success();
    }
}
