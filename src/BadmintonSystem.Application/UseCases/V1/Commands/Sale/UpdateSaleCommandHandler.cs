using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Sale;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Sale;

public sealed class UpdateSaleCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Sale, Guid> saleRepository)
    : ICommandHandler<Command.UpdateSaleCommand, Response.SaleResponse>
{
    public async Task<Result<Response.SaleResponse>> Handle
        (Command.UpdateSaleCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Sale sale = await saleRepository.FindByIdAsync(request.Data.Id, cancellationToken)
                                    ?? throw new SaleException.SaleNotFoundException(request.Data.Id);

        sale.Name = request.Data.Name ?? sale.Name;
        sale.Percent = request.Data.Percent ?? sale.Percent;
        sale.StartDate = request.Data.StartDate ?? sale.StartDate;
        sale.EndDate = request.Data.EndDate ?? sale.EndDate;
        sale.IsActive = request.Data.IsActive == 1 ? ActiveEnum.IsActive : ActiveEnum.NotActive;

        Response.SaleResponse? result = mapper.Map<Response.SaleResponse>(sale);

        return Result.Success(result);
    }
}
