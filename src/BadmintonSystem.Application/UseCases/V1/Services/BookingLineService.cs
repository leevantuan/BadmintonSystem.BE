﻿using System.Runtime.CompilerServices;
using System.Text;
using AutoMapper;
using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Price;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Services;

public sealed class BookingLineService(
    ApplicationDbContext context,
    IMapper mapper,
    IRedisService redisService,
    IRepositoryBase<YardPrice, Guid> pricesRepository)
    : IBookingLineService
{
    public async Task<decimal> CreateBookingLines
        (Guid bookingId, List<Guid> yardPriceIds, CancellationToken cancellationToken)
    {
        string priceColumns = StringExtension
            .TransformPropertiesToSqlAliases<Price,
                Response.PriceResponse>();

        string yardPriceColumns = StringExtension
            .TransformPropertiesToSqlAliases<YardPrice,
                Contract.Services.V1.YardPrice.Response.YardPriceResponse>();

        var priceQueryBuilder = new StringBuilder();
        priceQueryBuilder.Append(
            $@"SELECT {priceColumns}, {yardPriceColumns}
            FROM ""{nameof(YardPrice)}"" AS yardPrice
            JOIN ""{nameof(Price)}"" AS price
            ON price.""{nameof(Price.Id)}"" = yardPrice.""{nameof(YardPrice.PriceId)}""
            AND price.""{nameof(Price.IsDeleted)}"" = false
            WHERE yardPrice.""{nameof(YardPrice.IsDeleted)}"" = false
            AND yardPrice.""{nameof(YardPrice.Id)}"" = ANY (ARRAY[");

        foreach (Guid value in yardPriceIds)
        {
            priceQueryBuilder.Append($@"'{value}', ");
        }

        priceQueryBuilder.Length -= 2;

        priceQueryBuilder.Append("]::UUID[] ) ");

        List<Contract.Services.V1.BookingLine.Response.YardPriceDetailSql> queryResult = await pricesRepository
            .ExecuteSqlQuery<Contract.Services.V1.BookingLine.Response.YardPriceDetailSql>(
                FormattableStringFactory.Create(priceQueryBuilder.ToString()))
            .ToListAsync(cancellationToken);

        var prices = queryResult.GroupBy(x => x.YardPrice_Id)
            .Select(x => new Contract.Services.V1.BookingLine.Response.PriceDetailResponse
            {
                YardPriceId = x.Key,
                TotalPrice = x.First().Price_YardPrice ?? 0
            }).ToList();

        var pricesDictionary = prices.ToDictionary(p => p.YardPriceId, p => p.TotalPrice);

        if (yardPriceIds.Count > 0)
        {
            foreach (Guid yardPriceId in yardPriceIds)
            {
                YardPrice yardPrice =
                    await context.YardPrice.FirstOrDefaultAsync(x => x.Id == yardPriceId, cancellationToken)
                    ?? throw new YardPriceException.YardPriceNotFoundException(yardPriceId);

                if (yardPrice.IsBooking == BookingEnum.BOOKED)
                {
                    throw new ApplicationException($"Already booked yard price for {yardPriceId}");
                }

                var bookingLine = new BookingLine
                {
                    BookingId = bookingId,
                    YardPriceId = yardPriceId,
                    TotalPrice = pricesDictionary[yardPriceId]
                };

                yardPrice.IsBooking = BookingEnum.BOOKED;

                BookingLine? bookingLineEntities = mapper.Map<BookingLine>(bookingLine);

                context.BookingLine.Add(bookingLineEntities);

                await context.SaveChangesAsync(cancellationToken);

                string endpoint = "/api/v1/yard-prices/filter-by-date";

                var cacheKey = StringExtension.GenerateCacheKeyFromRequest(endpoint, yardPrice.EffectiveDate);

                await redisService.DeleteByKeyAsync(cacheKey);
            }
        }

        await context.SaveChangesAsync(cancellationToken);

        decimal result = prices.Sum(x => x.TotalPrice);

        return result;
    }

    public async Task DeleteBookingLinesByBookingId(Guid bookingId, CancellationToken cancellationToken)
    {
        var deleteBookingLineQueryBuilder = new StringBuilder();
        deleteBookingLineQueryBuilder.Append($@"DELETE FROM ""{nameof(BookingLine)}""
            WHERE ""{nameof(BookingLine.BookingId)}"" = '{bookingId}'");

        await context.Database.ExecuteSqlRawAsync(deleteBookingLineQueryBuilder.ToString(), cancellationToken);
    }
}
