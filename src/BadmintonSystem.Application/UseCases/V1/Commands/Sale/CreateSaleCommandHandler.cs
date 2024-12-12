using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Sale;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Sale;

public sealed class CreateSaleCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Sale, Guid> saleRepository)
    : ICommandHandler<Command.CreateSaleCommand, Response.SaleResponse>
{
    public Task<Result<Response.SaleResponse>> Handle
        (Command.CreateSaleCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Sale sale = mapper.Map<Domain.Entities.Sale>(request.Data);

        sale.IsActive = request.Data.IsActive == 1 ? ActiveEnum.IsActive : ActiveEnum.NotActive;

        saleRepository.Add(sale);

        Response.SaleResponse? result = mapper.Map<Response.SaleResponse>(sale);

        return Task.FromResult(Result.Success(result));
    }
}
