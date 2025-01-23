using BadmintonSystem.Contract.Services.V1.Bill;

namespace BadmintonSystem.Application.Abstractions;

public interface IBookingHub
{
    Task SendBookingMessageToUserAsync(string userId, Response.BookingHubResponse message);

    Task BookingByUserAsync(Response.BookingHubResponse message);

    Task ReserveBookingByUserAsync(Response.BookingHubResponse message);
}
