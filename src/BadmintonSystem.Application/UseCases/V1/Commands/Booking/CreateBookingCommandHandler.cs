using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Booking;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Booking;

public sealed class CreateBookingCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Booking, Guid> bookingRepository)
    : ICommandHandler<Command.CreateBookingCommand, Response.BookingResponse>
{
    public Task<Result<Response.BookingResponse>> Handle
        (Command.CreateBookingCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Booking booking = mapper.Map<Domain.Entities.Booking>(request.Data);

        booking.BookingStatus = BookingStatusEnum.Pending;
        booking.PaymentStatus = PaymentStatusEnum.Unpaid;

        bookingRepository.Add(booking);

        Response.BookingResponse? result = mapper.Map<Response.BookingResponse>(booking);

        return Task.FromResult(Result.Success(result));
    }
}
