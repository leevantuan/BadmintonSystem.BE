using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Price;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Price;

public sealed class CreatePriceCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Price, Guid> priceRepository)
    : ICommandHandler<Command.CreatePriceCommand, Response.PriceResponse>
{
    public Task<Result<Response.PriceResponse>> Handle
        (Command.CreatePriceCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Price? priceDefaultExists = context.Price.FirstOrDefault(x => x.IsDefault == DefaultEnum.TRUE);

        Domain.Entities.Price price = mapper.Map<Domain.Entities.Price>(request.Data);

        price.IsDefault = (DefaultEnum)request.Data.IsDefault;

        if (priceDefaultExists != null)
        {
            price.IsDefault = (int)DefaultEnum.FALSE;
        }

        priceRepository.Add(price);

        Response.PriceResponse? result = mapper.Map<Response.PriceResponse>(price);

        return Task.FromResult(Result.Success(result));
    }
}
