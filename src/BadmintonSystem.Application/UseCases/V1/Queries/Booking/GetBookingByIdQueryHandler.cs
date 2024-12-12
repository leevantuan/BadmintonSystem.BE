using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Booking;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Booking;

public sealed class GetBookingByIdQueryHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Booking, Guid> bookingRepository)
    : IQueryHandler<Query.GetBookingByIdQuery, Response.BookingResponse>
{
    public async Task<Result<Response.BookingResponse>> Handle
        (Query.GetBookingByIdQuery request, CancellationToken cancellationToken)
    {
        Domain.Entities.Booking booking = await bookingRepository.FindByIdAsync(request.Id, cancellationToken)
                                          ?? throw new BookingException.BookingNotFoundException(request.Id);

        Response.BookingResponse? result = mapper.Map<Response.BookingResponse>(booking);

        return Result.Success(result);
    }
}
