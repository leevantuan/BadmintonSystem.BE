using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.InventoryReceipt;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;

namespace BadmintonSystem.Application.UseCases.V1.Queries.InventoryReceipt;

public sealed class GetInventoryReceiptByIdQueryHandler(
    IMapper mapper,
    IRepositoryBase<Domain.Entities.InventoryReceipt, Guid> inventoryReceiptRepository)
    : IQueryHandler<Query.GetInventoryReceiptByIdQuery, Response.InventoryReceiptDetailResponse>
{
    public async Task<Result<Response.InventoryReceiptDetailResponse>> Handle
        (Query.GetInventoryReceiptByIdQuery request, CancellationToken cancellationToken)
    {
        Domain.Entities.InventoryReceipt inventoryReceipt =
            await inventoryReceiptRepository.FindByIdAsync(request.Id, cancellationToken)
            ?? throw new InventoryReceiptException.InventoryReceiptNotFoundException(request.Id);

        Response.InventoryReceiptDetailResponse? result =
            mapper.Map<Response.InventoryReceiptDetailResponse>(inventoryReceipt);

        return Result.Success(result);
    }
}
