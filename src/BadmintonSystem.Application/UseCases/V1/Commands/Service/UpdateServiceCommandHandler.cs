using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Service;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Service;

public sealed class UpdateServiceCommandHandler(
    IRepositoryBase<Domain.Entities.Service, Guid> serviceRepository)
    : ICommandHandler<Command.UpdateServiceCommand>
{
    public async Task<Result> Handle
        (Command.UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Service service = await serviceRepository.FindByIdAsync(request.Data.Id, cancellationToken)
                                          ?? throw new ServiceException.ServiceNotFoundException(request.Data.Id);

        if (request.Data.Name != null && request.Data.Name != service.Name)
        {
            Domain.Entities.Service? isNameExists =
                await serviceRepository.FindSingleAsync(x => x.Name == request.Data.Name, cancellationToken);

            if (isNameExists != null)
            {
                return Result.Failure(new Error("400", "Name Exists!"));
            }
        }

        service.Name = request.Data.Name ?? service.Name;
        service.Description = request.Data.Description ?? service.Description;
        service.ImageLink = request.Data.ImageLink ?? service.ImageLink;
        service.SellingPrice = request.Data.SellingPrice ?? service.SellingPrice;
        service.PurchasePrice = request.Data.PurchasePrice ?? service.PurchasePrice;
        service.Unit = request.Data.Unit ?? service.Unit;

        return Result.Success();
    }
}
