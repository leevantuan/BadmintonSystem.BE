using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;
using static BadmintonSystem.Contract.Services.V1.Booking.Command;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Booking;

public sealed class CreateUrlBookingByChatCommandHandler(
    ApplicationDbContext context)
    : ICommandHandler<CreateUrlBookingByChatCommand, string>
{
    public async Task<Result<string>> Handle(CreateUrlBookingByChatCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users.Where(x => x.Email == request.Data.Email)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new ApplicationException("Email not found");

        var date = request.Data.BookingDate;
        var startTime = request.Data.StartTime;
        var endTime = request.Data.EndTime;
        var tenant = request.Data.Tenant;

        string url = "https://bookingweb.shop/api/v1/bookings/handle-create-booking?" +
             $"userName={Uri.EscapeDataString(user.UserName ?? string.Empty)}&" +
             $"userId={Uri.EscapeDataString(user.Id.ToString())}&" +
             $"phoneNumber={Uri.EscapeDataString(user.PhoneNumber ?? string.Empty)}&" +
             $"email={Uri.EscapeDataString(user.Email ?? string.Empty)}&" +
             $"date={Uri.EscapeDataString(date)}&" +
             $"startTime={Uri.EscapeDataString(startTime)}&" +
             $"endTime={Uri.EscapeDataString(endTime)}&" +
             $"tenant={Uri.EscapeDataString(tenant)}";

        return Result.Success(url);
    }
}
