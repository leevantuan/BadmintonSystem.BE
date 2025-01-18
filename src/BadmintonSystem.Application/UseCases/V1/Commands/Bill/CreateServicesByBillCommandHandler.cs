using AutoMapper;
using BadmintonSystem.Application.UseCases.V1.Services;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Bill;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Bill;

public sealed class CreateServicesByBillCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IServiceLineService serviceLineService,
    IRepositoryBase<Domain.Entities.Bill, Guid> billRepository)
    : ICommandHandler<Command.CreateServicesByBillCommand>
{
    public async Task<Result> Handle(Command.CreateServicesByBillCommand request, CancellationToken cancellationToken)
    {
        _ = await billRepository.FindByIdAsync(request.BillId, cancellationToken)
            ?? throw new BillException.BillNotFoundException(request.BillId);

        await serviceLineService.CreateServiceLine(request.BillId, request.Data, cancellationToken);

        return Result.Success();
    }
}
