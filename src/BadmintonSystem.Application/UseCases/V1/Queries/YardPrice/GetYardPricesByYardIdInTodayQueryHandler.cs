using System.Runtime.CompilerServices;
using System.Text;
using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.YardPrice;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.YardPrice;

public sealed class GetYardPricesByYardIdInTodayQueryHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.YardPrice, Guid> yardPriceRepository)
    : IQueryHandler<Query.GetYardPricesByYardIdInTodayQuery, Response.YardPricesByDateDetailResponse>
{
    public async Task<Result<Response.YardPricesByDateDetailResponse>> Handle
        (Query.GetYardPricesByYardIdInTodayQuery request, CancellationToken cancellationToken)
    {
        DateTime date = DateTime.Now;

        string yardColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.Yard,
                Contract.Services.V1.Yard.Response.YardResponse>();

        string yardPriceColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.YardPrice,
                Response.YardPriceResponse>();

        string yardTypeColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.YardType,
                Contract.Services.V1.YardType.Response.YardTypeResponse>();

        string priceColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.Price,
                Contract.Services.V1.Price.Response.PriceResponse>();

        string timeSlotColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.TimeSlot,
                Contract.Services.V1.TimeSlot.Response.TimeSlotResponse>();

        var baseQueryBuilder = new StringBuilder();
        baseQueryBuilder.Append(
            $@"FROM ""{nameof(Domain.Entities.YardPrice)}"" AS yardPrice
                LEFT JOIN ""{nameof(Domain.Entities.Price)}"" AS price
                ON yardPrice.""{nameof(Domain.Entities.YardPrice.PriceId)}"" = price.""{nameof(Domain.Entities.Price.Id)}""
                AND price.""{nameof(Domain.Entities.Price.IsDeleted)}"" = false
                JOIN ""{nameof(Domain.Entities.Yard)}"" AS yard
                ON yard.""{nameof(Domain.Entities.Yard.Id)}"" = yardPrice.""{nameof(Domain.Entities.YardPrice.YardId)}""
                AND yard.""{nameof(Domain.Entities.Yard.IsDeleted)}"" = false
                JOIN ""{nameof(Domain.Entities.YardType)}"" AS yardType
                ON yardType.""{nameof(Domain.Entities.YardType.Id)}"" = yard.""{nameof(Domain.Entities.Yard.YardTypeId)}""
                AND yardType.""{nameof(Domain.Entities.YardType.IsDeleted)}"" = false
                JOIN ""{nameof(Domain.Entities.TimeSlot)}"" AS timeSlot
                ON timeSlot.""{nameof(Domain.Entities.TimeSlot.Id)}"" = yardPrice.""{nameof(Domain.Entities.YardPrice.TimeSlotId)}""
                AND timeSlot.""{nameof(Domain.Entities.TimeSlot.IsDeleted)}"" = false
                WHERE yardPrice.""{nameof(Domain.Entities.YardPrice.IsDeleted)}"" = false
                AND yardPrice.""{nameof(Domain.Entities.YardPrice.EffectiveDate)}""::DATE = '{date.Date}'
                AND yard.""{nameof(Domain.Entities.Yard.Id)}"" = '{request.YardId}' ");

        var yardPriceQueryBuilder = new StringBuilder();
        yardPriceQueryBuilder.Append(
            $@"SELECT {yardColumns}, {yardPriceColumns}, {yardTypeColumns}, {priceColumns}, {timeSlotColumns} ");
        yardPriceQueryBuilder.Append(" \n");
        yardPriceQueryBuilder.Append(baseQueryBuilder.ToString());

        List<Response.YardPriceDetailSql> queryResult = await yardPriceRepository
            .ExecuteSqlQuery<Response.YardPriceDetailSql>(
                FormattableStringFactory.Create(yardPriceQueryBuilder.ToString()))
            .ToListAsync(cancellationToken);

        // Group by
        Response.YardPricesByDateDetailResponse? result = queryResult.GroupBy(p => p.Yard_Id)
            .Select(g => new Response.YardPricesByDateDetailResponse
            {
                Yard = g.Where(x => x.Yard_Id != null)
                    .Select(s => new Contract.Services.V1.Yard.Response.YardResponse
                    {
                        Id = s.Yard_Id ?? Guid.Empty,
                        Name = s.Yard_Name ?? string.Empty,
                        YardTypeId = s.Yard_YardTypeId ?? Guid.Empty,
                        IsStatus = s.Yard_IsStatus ?? 0
                    }).OrderBy(x => x.Name)
                    .FirstOrDefault(),

                YardPricesDetails = g.Select(x =>
                    new Response.YardPricesByDateDetail
                    {
                        Id = x.YardPrice_Id ?? Guid.Empty,
                        YardId = x.YardPrice_YardId ?? Guid.Empty,
                        TimeSlotId = x.YardPrice_TimeSlotId ?? Guid.Empty,
                        PriceId = x.YardPrice_PriceId,
                        EffectiveDate = x.YardPrice_EffectiveDate ?? DateTime.Now,
                        IsBooking = x.YardPrice_IsBooking ?? 0,
                        Price = x.Price_YardPrice ?? 0,
                        StartTime = x.TimeSlot_StartTime ?? TimeSpan.Zero,
                        EndTime = x.TimeSlot_EndTime ?? TimeSpan.Zero
                    }).OrderBy(x => x.StartTime).ToList()
            })
            .FirstOrDefault();

        return Result.Success(result);
    }
}
