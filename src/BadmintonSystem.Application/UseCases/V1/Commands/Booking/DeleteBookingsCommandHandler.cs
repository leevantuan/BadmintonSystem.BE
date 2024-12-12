using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Booking;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Booking;

public sealed class DeleteBookingsCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Booking, Guid> bookingRepository)
    : ICommandHandler<Command.DeleteBookingsCommand>
{
    public async Task<Result> Handle(Command.DeleteBookingsCommand request, CancellationToken cancellationToken)
    {
        List<Domain.Entities.Booking> bookings = new();

        foreach (string id in request.Ids)
        {
            var idValue = Guid.Parse(id);

            Domain.Entities.Booking booking = await bookingRepository.FindByIdAsync(idValue, cancellationToken)
                                              ?? throw new BookingException.BookingNotFoundException(idValue);

            bookings.Add(booking);
        }

        bookingRepository.RemoveMultiple(bookings);

        return Result.Success();
    }
}
