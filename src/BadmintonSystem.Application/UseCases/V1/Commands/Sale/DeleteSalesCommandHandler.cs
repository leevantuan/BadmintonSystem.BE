using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Sale;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Sale;

public sealed class DeleteSalesCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Sale, Guid> saleRepository)
    : ICommandHandler<Command.DeleteSalesCommand>
{
    public async Task<Result> Handle(Command.DeleteSalesCommand request, CancellationToken cancellationToken)
    {
        List<Domain.Entities.Sale> sales = new();

        foreach (string id in request.Ids)
        {
            var idValue = Guid.Parse(id);

            Domain.Entities.Sale sale = await saleRepository.FindByIdAsync(idValue, cancellationToken)
                                        ?? throw new SaleException.SaleNotFoundException(idValue);

            sales.Add(sale);
        }

        saleRepository.RemoveMultiple(sales);

        return Result.Success();
    }
}
