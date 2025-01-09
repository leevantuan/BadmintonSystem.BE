using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.YardPrice;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.YardPrice;

public sealed class CreateYardPriceCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.YardPrice, Guid> yardPriceRepository)
    : ICommandHandler<Command.CreateYardPriceCommand, Response.YardPriceResponse>
{
    public Task<Result<Response.YardPriceResponse>> Handle
        (Command.CreateYardPriceCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.YardPrice yardPrice = mapper.Map<Domain.Entities.YardPrice>(request.Data);

        yardPrice.IsBooking = (BookingEnum)request.Data.IsBooking;

        // Guid query = (from yard in context.Yard
        //     join yardType in context.YardType on yard.YardTypeId equals yardType.Id
        //     join price in context.Price on yardType.PriceId equals price.Id
        //     where yard.Id == request.Data.YardId
        //     select price.Id).FirstOrDefault();
        //
        // yardPrice.PriceId = query;

        yardPriceRepository.Add(yardPrice);

        Response.YardPriceResponse? result = mapper.Map<Response.YardPriceResponse>(yardPrice);

        return Task.FromResult(Result.Success(result));
    }
}
