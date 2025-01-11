using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Price;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Price;

public sealed class GetPricesWithDayOfWeekQueryHandler(
    ApplicationDbContext context)
    : IQueryHandler<Query.GetPricesWithDayOfWeekQuery, List<Response.GetListPriceResponse>>
{
    public async Task<Result<List<Response.GetListPriceResponse>>> Handle
        (Query.GetPricesWithDayOfWeekQuery request, CancellationToken cancellationToken)
    {
        var dayOfWeekOrder = new Dictionary<string, int>
        {
            { "MONDAY", 1 },
            { "TUESDAY", 2 },
            { "WEDNESDAY", 3 },
            { "THURSDAY", 4 },
            { "FRIDAY", 5 },
            { "SATURDAY", 6 },
            { "SUNDAY", 7 }
        };

        var query =
            from price in context.Price
            join yardType in context.YardType
                on price.YardTypeId equals yardType.Id
                into priceByYardType
            from yardType in priceByYardType.DefaultIfEmpty()
            where price.IsDeleted == false
                  && yardType.IsDeleted == false
                  && price.DayOfWeek != null
            select new { price, yardType };

        List<Response.GetListPriceResponse> results = await query
            .AsNoTracking()
            .GroupBy(x => x.yardType.Name)
            .Select(group => new Response.GetListPriceResponse
            {
                YardType = group.Key ?? string.Empty,

                PriceByDayOfWeeks = query
                    .AsNoTracking()
                    .GroupBy(x => x.price.DayOfWeek)
                    .Select(x => new Response.ListPriceByYardType
                    {
                        DayOfWeek = x.Key ?? string.Empty,
                        PriceDetails = x
                            .Select(g => new Response.ListPriceDetail
                            {
                                Id = g.price.Id,
                                StartTime = g.price.StartTime ?? TimeSpan.Zero,
                                EndTime = g.price.EndTime ?? TimeSpan.Zero,
                                YardPrice = g.price.YardPrice
                            }).OrderBy(p => p.StartTime).ToList()
                    }).ToList() ?? new List<Response.ListPriceByYardType>()
            })
            .ToListAsync(cancellationToken);

        var sortedResults =
            results.Select(r => new Response.GetListPriceResponse
            {
                YardType = r.YardType,
                PriceByDayOfWeeks =
                    r.PriceByDayOfWeeks.OrderBy(p => dayOfWeekOrder[p.DayOfWeek]).ToList()
            }).ToList();

        return Result.Success(sortedResults);
    }
}
