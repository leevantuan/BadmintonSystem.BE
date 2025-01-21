using System.Text;
using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Application.UseCases.V1.Services;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Booking;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;
using Response = BadmintonSystem.Contract.Services.V1.Bill.Response;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Booking;

public sealed class DeleteBookingByIdCommandHandler(
    ApplicationDbContext context,
    IYardPriceService yardPriceService,
    IBookingLineService bookingLineService,
    IBillService billService,
    IBookingHub bookingHub,
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

        List<Guid> yardPriceIds = await GetYardPriceIds(request.Id, cancellationToken);

        var deleteBookingQueryBuilder = new StringBuilder();
        deleteBookingQueryBuilder.Append($@"DELETE FROM ""{nameof(Domain.Entities.Booking)}""
            WHERE ""{nameof(Domain.Entities.Booking.Id)}"" = '{request.Id}'");

        await context.Database.ExecuteSqlRawAsync(deleteBookingQueryBuilder.ToString(), cancellationToken);

        await bookingHub.BookingByUserAsync(new Response.BookingHubResponse
        {
            Ids = yardPriceIds,
            Type = BookingEnum.UNBOOKED.ToString()
        });

        return Result.Success();
    }

    private async Task<List<Guid>> GetYardPriceIds(Guid bookingId, CancellationToken cancellationToken)
    {
        List<Guid> query = await (from booking in context.Booking
            join bookingLine in context.BookingLine on booking.Id equals bookingLine.BookingId
            where booking.Id == bookingId
            select bookingLine.YardPriceId).ToListAsync(cancellationToken);

        return query.ToList();
    }
}
