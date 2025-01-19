using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Price;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Price;

public sealed class CreatePriceCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Price, Guid> priceRepository)
    : ICommandHandler<Command.CreatePriceCommand, Response.PriceResponse>
{
    public async Task<Result<Response.PriceResponse>> Handle
        (Command.CreatePriceCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Price? priceDefaultExists =
            await context.Price.FirstOrDefaultAsync(x => x.IsDefault == DefaultEnum.TRUE, cancellationToken);

        Domain.Entities.Price price = mapper.Map<Domain.Entities.Price>(request.Data);

        price.IsDefault = (DefaultEnum)request.Data.IsDefault;

        if (priceDefaultExists != null)
        {
            price.IsDefault = (int)DefaultEnum.FALSE;
        }

        priceRepository.Add(price);

        Response.PriceResponse? result = mapper.Map<Response.PriceResponse>(price);

        return Result.Success(result);
    }
}
