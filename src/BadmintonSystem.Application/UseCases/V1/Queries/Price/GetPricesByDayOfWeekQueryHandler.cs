using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Price;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Price;

public sealed class GetPricesByDayOfWeekQueryHandler(
    ApplicationDbContext context)
    : IQueryHandler<Query.GetPricesByDayOfWeekQuery, Response.GetListPriceResponse>
{
    public Task<Result<Response.GetListPriceResponse>> Handle
        (Query.GetPricesByDayOfWeekQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
