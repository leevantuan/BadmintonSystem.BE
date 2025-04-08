using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.BookingHistory;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.BookingHistory;

public sealed class GetBookingHistoriesByDateQueryHandler(
    TenantDbContext context)
    : IQueryHandler<Query.GetBookingHistoriesByDateQuery, List<Response.BookingHistoryDetailResponse>>
{
    public async Task<Result<List<Response.BookingHistoryDetailResponse>>> Handle(Query.GetBookingHistoriesByDateQuery request, CancellationToken cancellationToken)
    {
        var bookingHistories = await context.BookingHistories
            .Where(x => x.UserId == request.UserId && x.PlayDate.Date == request.Date.Date)
            .OrderBy(x => x.PlayDate)
            .ToListAsync(cancellationToken);

        // Group by ClubName → StartTime, sau đó flatten về List<BookingHistoryDetailResponse>
        var distinctBookingHistories = bookingHistories
            .GroupBy(x => new { x.ClubName, x.StartTime })
            .Select(g => g.OrderBy(x => x.CreatedDate).First()) // hoặc Last() nếu muốn lấy mới nhất
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

        return Result.Success(distinctBookingHistories);
    }
}
