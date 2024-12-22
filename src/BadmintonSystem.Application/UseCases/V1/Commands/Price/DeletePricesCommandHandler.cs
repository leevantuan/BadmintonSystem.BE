using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Price;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Price;

public sealed class DeletePricesCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Price, Guid> priceRepository)
    : ICommandHandler<Command.DeletePricesCommand>
{
    public async Task<Result> Handle(Command.DeletePricesCommand request, CancellationToken cancellationToken)
    {
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
