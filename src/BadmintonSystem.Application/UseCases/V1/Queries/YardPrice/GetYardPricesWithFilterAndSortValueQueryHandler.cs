using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.YardPrice;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Queries.YardPrice;

public sealed class GetYardPricesWithFilterAndSortValueQueryHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.YardPrice, Guid> yardPriceRepository)
    : IQueryHandler<Query.GetYardPricesWithFilterAndSortValueQuery, PagedResult<Response.YardPriceDetailResponse>>
{
    public Task<Result<PagedResult<Response.YardPriceDetailResponse>>> Handle
        (Query.GetYardPricesWithFilterAndSortValueQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
