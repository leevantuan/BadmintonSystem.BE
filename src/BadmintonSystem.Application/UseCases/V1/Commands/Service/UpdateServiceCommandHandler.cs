using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Service;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Service;

public sealed class UpdateServiceCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Service, Guid> serviceRepository)
    : ICommandHandler<Command.UpdateServiceCommand, Response.ServiceResponse>
{
    public async Task<Result<Response.ServiceResponse>> Handle
        (Command.UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Service service = await serviceRepository.FindByIdAsync(request.Data.Id)
                                          ?? throw new ServiceException.ServiceNotFoundException(request.Data.Id);

        service.Name = request.Data.Name ?? service.Name;
        service.SellingPrice = request.Data.SellingPrice ?? service.SellingPrice;
        service.PurchasePrice = request.Data.PurchasePrice ?? service.PurchasePrice;

        Response.ServiceResponse? result = mapper.Map<Response.ServiceResponse>(service);

        return Result.Success(result);
    }
}
