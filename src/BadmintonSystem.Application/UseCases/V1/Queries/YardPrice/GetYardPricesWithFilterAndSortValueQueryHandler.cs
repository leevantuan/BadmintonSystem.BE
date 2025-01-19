using System.Runtime.CompilerServices;
using System.Text;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Enumerations;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.YardPrice;
using BadmintonSystem.Domain.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.YardPrice;

public sealed class GetYardPricesWithFilterAndSortValueQueryHandler(
    IRepositoryBase<Domain.Entities.YardPrice, Guid> yardPriceRepository)
    : IQueryHandler<Query.GetYardPricesWithFilterAndSortValueQuery, PagedResult<Response.YardPriceDetailResponse>>
{
    public async Task<Result<PagedResult<Response.YardPriceDetailResponse>>> Handle
        (Query.GetYardPricesWithFilterAndSortValueQuery request, CancellationToken cancellationToken)
    {
        int pageIndex = request.Data.PageIndex <= 0
            ? PagedResult<Domain.Entities.YardPrice>.DefaultPageIndex
            : request.Data.PageIndex;
        int pageSize = request.Data.PageSize <= 0
            ? PagedResult<Domain.Entities.YardPrice>.DefaultPageSize
            : request.Data.PageSize > PagedResult<Domain.Entities.YardPrice>.UpperPageSize
                ? PagedResult<Domain.Entities.YardPrice>.UpperPageSize
                : request.Data.PageSize;

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
                WHERE yardPrice.""{nameof(Domain.Entities.YardPrice.IsDeleted)}"" = false ");

        if (request.Data.FilterColumnAndMultipleValue.Any())
        {
            foreach (KeyValuePair<string, List<string>> item in request.Data.FilterColumnAndMultipleValue)
            {
                string key = YardPriceExtension.GetSortYardPriceProperty(item.Key);
                baseQueryBuilder.Append(
                    $@"AND ""{nameof(Domain.Entities.YardPrice)}"".""{key}""::TEXT ILIKE ANY (ARRAY[");

                foreach (string value in item.Value)
                {
                    baseQueryBuilder.Append($@"'%{value}%', ");
                }

                baseQueryBuilder.Length -= 2;

                baseQueryBuilder.Append("]) ");
            }
        }

        if (request.Data.SortColumnAndOrder.Any())
        {
            baseQueryBuilder.Append("ORDER BY ");
            foreach (KeyValuePair<string, SortOrder> item in request.Data.SortColumnAndOrder)
            {
                string key = YardPriceExtension.GetSortYardPriceProperty(item.Key);
                baseQueryBuilder.Append(item.Value == SortOrder.Descending
                    ? $@" ""{nameof(Domain.Entities.Price)}"".""{key}"" DESC, "
                    : $@" ""{nameof(Domain.Entities.Price)}"".""{key}"" ASC, ");
            }

            baseQueryBuilder.Length -= 2;
        }

        int totalCount = await TotalCount(baseQueryBuilder.ToString(), cancellationToken);

        var yardPriceQueryBuilder = new StringBuilder();
        yardPriceQueryBuilder.Append(
            $@"SELECT {yardColumns}, {yardPriceColumns}, {yardTypeColumns}, {priceColumns}, {timeSlotColumns} ");
        yardPriceQueryBuilder.Append(" \n");
        yardPriceQueryBuilder.Append(baseQueryBuilder.ToString());

        yardPriceQueryBuilder.Append($"\nOFFSET {(pageIndex - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY");

        List<Response.YardPriceDetailSql> queryResult = await yardPriceRepository
            .ExecuteSqlQuery<Response.YardPriceDetailSql>(
                FormattableStringFactory.Create(yardPriceQueryBuilder.ToString()))
            .ToListAsync(cancellationToken);

        var yardPricePagedResult =
            PagedResult<Response.YardPriceDetailResponse>.Create(
                GroupByData(queryResult),
                pageIndex,
                pageSize,
                totalCount);

        return Result.Success(yardPricePagedResult);
    }

    private List<Response.YardPriceDetailResponse> GroupByData(List<Response.YardPriceDetailSql> queryResult)
    {
        var results = queryResult.GroupBy(p => p.YardPrice_Id)
            .Select(g => new Response.YardPriceDetailResponse
            {
                Id = g.Key ?? Guid.Empty,
                YardId = g.First().YardPrice_YardId ?? Guid.Empty,
                TimeSlotId = g.First().YardPrice_TimeSlotId ?? Guid.Empty,
                PriceId = g.First().YardPrice_PriceId,
                EffectiveDate = g.First().YardPrice_EffectiveDate ?? DateTime.Now,
                IsBooking = g.First().YardPrice_IsBooking ?? 0,
                Yard = g.Where(x => x.Yard_Id != null)
                    .Select(s => new Contract.Services.V1.Yard.Response.YardResponse
                    {
                        Id = s.Yard_Id ?? Guid.Empty,
                        Name = s.Yard_Name ?? string.Empty,
                        YardTypeId = s.Yard_YardTypeId ?? Guid.Empty,
                        IsStatus = s.Yard_IsStatus ?? 0
                    })
                    .DistinctBy(s => s.Id)
                    .FirstOrDefault(),

                TimeSlot = g.Where(x => x.TimeSlot_Id != null)
                    .Select(s => new Contract.Services.V1.TimeSlot.Response.TimeSlotResponse
                    {
                        Id = s.TimeSlot_Id ?? Guid.Empty,
                        StartTime = s.TimeSlot_StartTime ?? TimeSpan.Zero,
                        EndTime = s.TimeSlot_EndTime ?? TimeSpan.Zero
                    })
                    .Distinct()
                    .FirstOrDefault(),

                Price = g.Where(x => x.Price_Id != null)
                    .Select(s => new Contract.Services.V1.Price.Response.PriceResponse
                    {
                        Id = s.Price_Id ?? Guid.Empty,
                        YardPrice = s.Price_YardPrice ?? 0,
                        IsDefault = s.Price_IsDefault ?? 0
                    })
                    .DistinctBy(s => s.Id)
                    .FirstOrDefault()
            })
            .ToList();

        return results;
    }

    private async Task<int> TotalCount(string baseQuery, CancellationToken cancellationToken)
    {
        var countQueryBuilder = new StringBuilder();
        countQueryBuilder.Append(
            $@"SELECT COUNT(*) AS ""{nameof(SqlResponse.TotalCountSqlResponse.TotalCount)}""");
        countQueryBuilder.Append(" \n");

        countQueryBuilder.Append(baseQuery);
        SqlResponse.TotalCountSqlResponse totalCountQueryResult = await yardPriceRepository
            .ExecuteSqlQuery<SqlResponse.TotalCountSqlResponse>(
                FormattableStringFactory.Create(countQueryBuilder.ToString()))
            .SingleAsync(cancellationToken);

        return totalCountQueryResult.TotalCount;
    }
}
