using AutoMapper;
using BadmintonSystem.Application.UseCases.V1.Services;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Bill;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Bill;

public sealed class CloseYardByBillCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IBillLineService billLineService,
    IRepositoryBase<BillLine, Guid> billLineRepository)
    : ICommandHandler<Command.CloseYardByBillCommand>
{
    public async Task<Result> Handle(Command.CloseYardByBillCommand request, CancellationToken cancellationToken)
    {
        _ = await billLineRepository.FindByIdAsync(request.Data.Id, cancellationToken)
            ?? throw new BillLineException.BillLineNotFoundException(request.Data.Id);

        await billLineService.CloseBillLineByBill(request.Data.Id, cancellationToken);

        return Result.Success();
    }
}
