using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Price;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Price;

public sealed class GetPricesWithFilterAndSortValueQueryHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Price, Guid> priceRepository)
    : IQueryHandler<Query.GetPricesWithFilterAndSortValueQuery, PagedResult<Response.PriceDetailResponse>>
{
    public Task<Result<PagedResult<Response.PriceDetailResponse>>> Handle
        (Query.GetPricesWithFilterAndSortValueQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
