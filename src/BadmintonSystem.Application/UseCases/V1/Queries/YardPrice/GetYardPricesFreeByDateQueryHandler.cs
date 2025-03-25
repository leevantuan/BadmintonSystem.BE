using System.Runtime.CompilerServices;
using System.Text;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.YardPrice;
using BadmintonSystem.Domain.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.YardPrice;

public sealed class GetYardPricesFreeByDateQueryHandler(
     IRepositoryBase<Domain.Entities.YardPrice, Guid> yardPriceRepository)
    : IQueryHandler<Query.GetYardPricesFreeByDateQuery, List<Response.YardPricesFreeByDateDetailResponse>>
{
    public async Task<Result<List<Response.YardPricesFreeByDateDetailResponse>>> Handle(Query.GetYardPricesFreeByDateQuery request, CancellationToken cancellationToken)
    {
        DateTime filterDate = request.Data.Date;
        //TimeSpan currentTime = DateTime.Now.TimeOfDay;
        //string currentTimeString = currentTime.ToString(@"hh\:mm\:ss");

        var baseQueryBuilder = new StringBuilder();
        baseQueryBuilder.Append($@"
                    SELECT yard.""{nameof(Domain.Entities.Yard.Name)}"" AS Yard_Name,
                           timeSlot.""{nameof(Domain.Entities.TimeSlot.StartTime)}"" AS TimeSlot_StartTime,
                           timeSlot.""{nameof(Domain.Entities.TimeSlot.EndTime)}"" AS TimeSlot_EndTime
                    FROM ""{nameof(Domain.Entities.YardPrice)}"" AS yardPrice
                    JOIN ""{nameof(Domain.Entities.Yard)}"" AS yard
                    ON yard.""{nameof(Domain.Entities.Yard.Id)}"" = yardPrice.""{nameof(Domain.Entities.YardPrice.YardId)}""
                    JOIN ""{nameof(Domain.Entities.TimeSlot)}"" AS timeSlot
                    ON timeSlot.""{nameof(Domain.Entities.TimeSlot.Id)}"" = yardPrice.""{nameof(Domain.Entities.YardPrice.TimeSlotId)}""
                    WHERE yardPrice.""{nameof(Domain.Entities.YardPrice.IsDeleted)}"" = false
                      AND timeSlot.""{nameof(Domain.Entities.TimeSlot.IsDeleted)}"" = false
                      AND yardPrice.""{nameof(Domain.Entities.YardPrice.EffectiveDate)}""::DATE = '{filterDate}'
                      AND yardPrice.""{nameof(Domain.Entities.YardPrice.IsBooking)}"" = 0
                      AND timeSlot.""{nameof(Domain.Entities.TimeSlot.EndTime)}"" BETWEEN '{request.Data.StartTime}' AND '{request.Data.EndTime}'
                      AND timeSlot.""{nameof(Domain.Entities.TimeSlot.EndTime)}"" >= '{request.Data.TimeNow}'
                    ORDER BY timeSlot.""{nameof(Domain.Entities.TimeSlot.StartTime)}""");

        List<Response.YardPricesFreeByDateDetailResponseSql> queryResult = await yardPriceRepository
            .ExecuteSqlQuery<Response.YardPricesFreeByDateDetailResponseSql>(
                FormattableStringFactory.Create(baseQueryBuilder.ToString()))
            .ToListAsync(cancellationToken);

        // Group by
        var results = queryResult.GroupBy(p => p.Yard_Name)
            .Select(g => new Response.YardPricesFreeByDateDetailResponse
            {
                YardName = g.First().Yard_Name,
                YardPrices = g.Select(p => new Response.YardPricesDetail
                {
                    StartTime = p.TimeSlot_StartTime ?? TimeSpan.Zero,
                    EndTime = p.TimeSlot_EndTime ?? TimeSpan.Zero,
                }).ToList()
            }).OrderBy(x => x.YardName)
            .ToList();

        return Result.Success(results);
    }
}
