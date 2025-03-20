using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Services;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Price;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Price;

public sealed class DeletePricesCommandHandler(
    IRedisService redisService,
    ICurrentTenantService currentTenantService,
    IRepositoryBase<Domain.Entities.Price, Guid> priceRepository)
    : ICommandHandler<Command.DeletePricesCommand>
{
    public async Task<Result> Handle(Command.DeletePricesCommand request, CancellationToken cancellationToken)
    {
        string endpoint = $"BMTSYS_{currentTenantService.Code.ToString()}-get-yard-prices-by-date";

        await redisService.DeletesAsync(endpoint);

        List<Domain.Entities.Price> prices = new();

        foreach (string id in request.Ids)
        {
            var idValue = Guid.Parse(id);

            Domain.Entities.Price price = await priceRepository.FindByIdAsync(idValue, cancellationToken)
                                          ?? throw new PriceException.PriceNotFoundException(idValue);

            prices.Add(price);
        }

        priceRepository.RemoveMultiple(prices);

        return Result.Success();
    }
}
