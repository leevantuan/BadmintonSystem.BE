using System.Text;
using BadmintonSystem.Application.UseCases.V1.Services;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Booking;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Booking;

public sealed class DeleteBookingByIdCommandHandler(
    ApplicationDbContext context,
    IYardPriceService yardPriceService,
    IBookingLineService bookingLineService,
    IBillService billService,
    IRepositoryBase<Domain.Entities.Booking, Guid> bookingRepository)
    : ICommandHandler<Command.DeleteBookingByIdCommand>
{
    public async Task<Result> Handle(Command.DeleteBookingByIdCommand request, CancellationToken cancellationToken)
    {
        _ = await bookingRepository.FindByIdAsync(request.Id, cancellationToken)
            ?? throw new BookingException.BookingNotFoundException(request.Id);

        await billService.DeleteBillByBookingId(request.Id, cancellationToken);
        await yardPriceService.UpdateYardPricesByBookingId(request.Id, cancellationToken);
        await bookingLineService.DeleteBookingLinesByBookingId(request.Id, cancellationToken);

        var deleteBookingQueryBuilder = new StringBuilder();
        deleteBookingQueryBuilder.Append($@"DELETE FROM ""{nameof(Domain.Entities.Booking)}""
            WHERE ""{nameof(Domain.Entities.Booking.Id)}"" = '{request.Id}'");

        await context.Database.ExecuteSqlRawAsync(deleteBookingQueryBuilder.ToString(), cancellationToken);

        return Result.Success();
    }
}
