using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Services;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.YardPrice;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;

namespace BadmintonSystem.Application.UseCases.V1.Commands.YardPrice;

public sealed class DeleteYardPricesCommandHandler(
    IRedisService redisService,
    ICurrentTenantService currentTenantService,
    IRepositoryBase<Domain.Entities.YardPrice, Guid> yardPriceRepository)
    : ICommandHandler<Command.DeleteYardPricesCommand>
{
    public async Task<Result> Handle(Command.DeleteYardPricesCommand request, CancellationToken cancellationToken)
    {
        string endpoint = $"BMTSYS_{currentTenantService.Code.ToString()}-get-yard-prices-by-date";

        await redisService.DeletesAsync(endpoint);

        List<Domain.Entities.YardPrice> yardPrices = new();

        foreach (string id in request.Ids)
        {
            var idValue = Guid.Parse(id);

            Domain.Entities.YardPrice yardPrice = await yardPriceRepository.FindByIdAsync(idValue, cancellationToken)
                                                  ?? throw new YardPriceException.YardPriceNotFoundException(idValue);

            yardPrices.Add(yardPrice);
        }

        yardPriceRepository.RemoveMultiple(yardPrices);

        return Result.Success();
    }
}
