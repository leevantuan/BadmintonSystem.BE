using BadmintonSystem.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BadmintonSystem.Application.UseCases.V1.Events.Booking;

public sealed class SendEmailByBookingCancelledEventHandler(
    ApplicationDbContext context,
    ISender sender,
    IHttpContextAccessor httpContextAccessor)
{
}
