using AutoMapper;
using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Price;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Price;

public sealed class UpdatePriceCommandHandler(
    IRedisService redisService,
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Price, Guid> priceRepository)
    : ICommandHandler<Command.UpdatePriceCommand, Response.PriceResponse>
{
    public async Task<Result<Response.PriceResponse>> Handle
        (Command.UpdatePriceCommand request, CancellationToken cancellationToken)
    {
        await redisService.DeletesAsync("BMTSYS_");

        Domain.Entities.Price? priceDefaultExists =
            await context.Price.FirstOrDefaultAsync(x => x.IsDefault == DefaultEnum.TRUE, cancellationToken);

        bool priceDefaultCorrect = request.Data.IsDefault == (int)DefaultEnum.TRUE;

        Domain.Entities.Price price =
            await priceRepository.FindByIdAsync(request.Data.Id, cancellationToken)
            ?? throw new PriceException.PriceNotFoundException(request.Data.Id);

        price.YardPrice = request.Data.YardPrice;
        price.Detail = request.Data.Detail;
        price.DayOfWeek = request.Data.DayOfWeek;
        price.StartTime = request.Data.StartTime;
        price.EndTime = request.Data.EndTime;
        price.YardTypeId = request.Data.YardTypeId;
        price.IsDefault = DefaultEnum.FALSE;

        if ((DefaultEnum)request.Data.IsDefault == DefaultEnum.TRUE && priceDefaultCorrect)
        {
            priceDefaultExists.IsDefault = DefaultEnum.FALSE;
            price.IsDefault = DefaultEnum.TRUE;
        }

        Response.PriceResponse? result = mapper.Map<Response.PriceResponse>(price);

        return Result.Success(result);
    }
}
