using System.Runtime.CompilerServices;
using System.Text;
using AutoMapper;
using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Application.UseCases.V1.Services;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Services;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.YardPrice;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BadmintonSystem.Application.UseCases.V1.Queries.YardPrice;

public sealed class GetYardPricesByDateQueryHandler(
    ApplicationDbContext context,
    ICurrentTenantService currentTenantService,
    IMapper mapper,
    IRedisService redisService,
    IHttpContextAccessor httpContextAccessor,
    IRepositoryBase<Domain.Entities.YardPrice, Guid> yardPriceRepository,
    IYardPriceService yardPriceService)
    : IQueryHandler<Query.GetYardPricesByDateQuery, List<Response.YardPricesByDateDetailResponse>>
{
    public async Task<Result<List<Response.YardPricesByDateDetailResponse>>> Handle
        (Query.GetYardPricesByDateQuery request, CancellationToken cancellationToken)
    {
        string endpoint = !string.IsNullOrEmpty(request.Tenant)
            ? $"{request.Tenant}-get-yard-prices-by-date"
            : $"{currentTenantService.Code.ToString()}-get-yard-prices-by-date";

        //endpoint = $"{currentTenantService.Code.ToString()}-get-yard-prices-by-date";

        string cacheKey = StringExtension.GenerateCacheKeyFromRequest(endpoint, request.Date);

        string cacheData = await redisService.GetAsync(cacheKey);

        if (!string.IsNullOrEmpty(cacheData))
        {
            List<Response.YardPricesByDateDetailResponse>? data =
                JsonConvert.DeserializeObject<List<Response.YardPricesByDateDetailResponse>>(cacheData);
            return data;
        }

        IQueryable<Domain.Entities.YardPrice>? effectiveDateIsExists =
            yardPriceRepository.FindAll(x => x.EffectiveDate.Date == request.Date.Date);

        if (!effectiveDateIsExists.Any())
        {
            await yardPriceService.CreateYardPrice(request.Date, request.UserId, cancellationToken);
        }

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

        DateTime filterDate = request.Date.Date;

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
                AND yardPrice.""{nameof(Domain.Entities.YardPrice.EffectiveDate)}""::DATE = '{filterDate}'");

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
        var results = queryResult.GroupBy(p => p.Yard_Id)
            .Select(g => new Response.YardPricesByDateDetailResponse
            {
                Yard = g.Where(x => x.Yard_Id != null)
                    .Select(s => new Contract.Services.V1.Yard.Response.YardResponse
                    {
                        Id = s.Yard_Id ?? Guid.Empty,
                        Name = s.Yard_Name ?? string.Empty,
                        YardTypeId = s.Yard_YardTypeId ?? Guid.Empty,
                        IsStatus = s.Yard_IsStatus ?? 0
                    })
                    .DistinctBy(s => s.Id).OrderBy(x => x.Name)
                    .FirstOrDefault(),

                YardPricesDetails = g.Select(x => new Response.YardPricesByDateDetail
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
            }).OrderBy(x => x.Yard.Name)
            .ToList();

        await redisService.SetAsync(cacheKey, results, TimeSpan.FromMinutes(30));

        return Result.Success(results);
    }
}
