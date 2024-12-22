using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.YardPrice;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.YardPrice;

public sealed class UpdateYardPriceCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.YardPrice, Guid> yardPriceRepository)
    : ICommandHandler<Command.UpdateYardPriceCommand, Response.YardPriceResponse>
{
    public async Task<Result<Response.YardPriceResponse>> Handle
        (Command.UpdateYardPriceCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.YardPrice yardPrice =
            await yardPriceRepository.FindByIdAsync(request.Data.Id, cancellationToken)
            ?? throw new YardPriceException.YardPriceNotFoundException(request.Data.Id);

        yardPrice.YardId = request.Data.YardId;
        yardPrice.PriceId = request.Data.PriceId ?? yardPrice.PriceId;
        yardPrice.TimeSlotId = request.Data.TimeSlotId;
        yardPrice.EffectiveDate = request.Data.EffectiveDate;
        yardPrice.IsBooking = (BookingEnum)request.Data.IsBooking;

        Response.YardPriceResponse? result = mapper.Map<Response.YardPriceResponse>(yardPrice);

        return Result.Success(result);
    }
}
