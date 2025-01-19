using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Sale;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Sale;

public sealed class GetSaleByIdQueryHandler(
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Sale, Guid> saleRepository)
    : IQueryHandler<Query.GetSaleByIdQuery, Response.SaleResponse>
{
    public async Task<Result<Response.SaleResponse>> Handle
        (Query.GetSaleByIdQuery request, CancellationToken cancellationToken)
    {
        Domain.Entities.Sale sale = await saleRepository.FindByIdAsync(request.Id, cancellationToken)
                                    ?? throw new SaleException.SaleNotFoundException(request.Id);

        Response.SaleResponse? result = mapper.Map<Response.SaleResponse>(sale);

        return Result.Success(result);
    }
}
