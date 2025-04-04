using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.BookingHistory;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.BookingHistory;

public sealed class GetBookingHistoriesWithFilterAndSortValueQueryHandler(
    TenantDbContext context)
    : IQueryHandler<Query.GetBookingHistoriesWithFilterAndSortValueQuery, PagedResult<Response.BookingHistoryDetailResponse>>
{
    public async Task<Result<PagedResult<Response.BookingHistoryDetailResponse>>> Handle(Query.GetBookingHistoriesWithFilterAndSortValueQuery request, CancellationToken cancellationToken)
    {
        int pageIndex = request.Data.PageIndex <= 0
            ? PagedResult<Domain.Entities.Category>.DefaultPageIndex
            : request.Data.PageIndex;
        int pageSize = request.Data.PageSize <= 0
            ? PagedResult<Domain.Entities.Category>.DefaultPageSize
            : request.Data.PageSize > PagedResult<Domain.Entities.Category>.UpperPageSize
                ? PagedResult<Domain.Entities.Category>.UpperPageSize
                : request.Data.PageSize;

        var bookingHistories = await context.BookingHistories.Where(x => x.UserId == request.UserId).OrderBy(x => x.CreatedDate).ToListAsync(cancellationToken);
        var bokingHistorieConvert = bookingHistories
            .Select(x => new Response.BookingHistoryDetailResponse
            {
                Id = x.Id,
                ClubName = x.ClubName,
                StartTime = x.StartTime,
                PlayDate = x.PlayDate,
                CreatedDate = x.CreatedDate,
                TotalPrice = x.TotalPrice,
                PaymentStatus = x.PaymentStatus == PaymentStatusEnum.Unpaid ? 2 : 1,
                TenantCode = x.TenantCode,
                UserId = x.UserId,
                BookingId = x.BookingId,
            }).ToList();

        var result = PagedResult<Response.BookingHistoryDetailResponse>.Create(
            bokingHistorieConvert, pageIndex, pageSize, bookingHistories.Count());

        return Result.Success(result);
    }
}
