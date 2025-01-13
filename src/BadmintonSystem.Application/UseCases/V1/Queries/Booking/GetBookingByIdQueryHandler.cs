using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Booking;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Booking;

public sealed class GetBookingByIdQueryHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Booking, Guid> bookingRepository)
    : IQueryHandler<Query.GetBookingByIdQuery, Response.GetBookingDetailResponse>
{
    public async Task<Result<Response.GetBookingDetailResponse>> Handle
        (Query.GetBookingByIdQuery request, CancellationToken cancellationToken)
    {
        _ = await bookingRepository.FindByIdAsync(request.Id, cancellationToken)
            ?? throw new BookingException.BookingNotFoundException(request.Id);

        var query = from booking in context.Booking
            join bookingLine in context.BookingLine
                on booking.Id equals bookingLine.BookingId
            join yardPrice in context.YardPrice
                on bookingLine.YardPriceId equals yardPrice.Id
            join yard in context.Yard
                on yardPrice.YardId equals yard.Id
            join timeSlot in context.TimeSlot
                on yardPrice.TimeSlotId equals timeSlot.Id
            where booking.Id == request.Id
            select new { booking, bookingLine, yardPrice, yard, timeSlot };

        Response.GetBookingDetailResponse? result = await query.AsNoTracking()
            .GroupBy(g => g.booking.Id)
            .Select(g => new Response.GetBookingDetailResponse
            {
                Id = g.Key,
                BookingDate = g.First().booking.BookingDate,
                EffectiveDate = g.First().yardPrice.EffectiveDate,
                BookingTotal = g.First().booking.BookingTotal,
                OriginalPrice = g.First().booking.OriginalPrice,
                UserId = g.First().booking.UserId ?? Guid.Empty,
                SaleId = g.First().booking.SaleId ?? Guid.Empty,
                BookingStatus = (int)g.First().booking.BookingStatus,
                PaymentStatus = (int)g.First().booking.PaymentStatus,
                FullName = g.First().booking.FullName,
                PhoneNumber = g.First().booking.PhoneNumber,
                User = g.Select(s => new Response.UserWithBooking
                    {
                        FullName = g.First().booking.FullName ?? string.Empty,
                        PhoneNumber = g.First().booking.PhoneNumber ?? string.Empty
                    })
                    .FirstOrDefault(),

                BookingLines = g.Where(x => x.bookingLine.Id != null)
                    .Select(s => new Response.BookingLineDetail
                    {
                        StartTime = s.timeSlot.StartTime,
                        EndTime = s.timeSlot.EndTime,
                        YardName = s.yard.Name,
                        Price = s.bookingLine.TotalPrice
                    })
                    .ToList()
            }).FirstOrDefaultAsync(cancellationToken);

        return Result.Success(result);
    }
}
