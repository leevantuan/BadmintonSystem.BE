using AutoMapper;
using BadmintonSystem.Application.UseCases.V1.Services;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Service;
using BadmintonSystem.Domain.Abstractions.Repositories;
using Request = BadmintonSystem.Contract.Services.V1.Service.Request;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Service;

public sealed class CreateServiceCommandHandler(
    IMapper mapper,
    IOriginalQuantityService originalQuantityService,
    IRepositoryBase<Domain.Entities.Service, Guid> serviceRepository)
    : ICommandHandler<Command.CreateServicesCommand>
{
    public async Task<Result> Handle
        (Command.CreateServicesCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Service serviceEntities = mapper.Map<Domain.Entities.Service>(request.Data);

        if (request.Data.IsWholeSale == 0)
        {
            Domain.Entities.Service? isNameExists =
                await serviceRepository.FindSingleAsync(x => x.Name == request.Data.ServiceDetails.First().Name,
                    cancellationToken);

            if (isNameExists != null)
            {
                return Result.Failure(new Error("400", "Name Exists!"));
            }

            serviceEntities.OriginalQuantityId = null;
            serviceEntities.QuantityPrinciple = null;
            serviceEntities.Name = request.Data.ServiceDetails.First().Name;
            serviceEntities.PurchasePrice = request.Data.ServiceDetails.First().PurchasePrice;
            serviceEntities.SellingPrice = request.Data.ServiceDetails.First().SellingPrice;
            serviceEntities.Unit = request.Data.ServiceDetails.First().Unit;

            serviceRepository.Add(serviceEntities);

            return Result.Success();
        }

        var originalQuantityId = Guid.NewGuid();

        await originalQuantityService.CreateOriginalQuantity(originalQuantityId, request.Data.QuantityInStock,
            cancellationToken);

        foreach (Request.ServiceDetail service in request.Data.ServiceDetails)
        {
            Domain.Entities.Service? isNameExists =
                await serviceRepository.FindSingleAsync(x => x.Name == service.Name, cancellationToken);

            if (isNameExists != null)
            {
                return Result.Failure(new Error("400", "Name Exists!"));
            }

            Domain.Entities.Service? entities = mapper.Map<Domain.Entities.Service>(request.Data);

            entities.OriginalQuantityId = originalQuantityId;
            entities.QuantityPrinciple = service.QuantityPrinciple;
            entities.Name = service.Name;
            entities.PurchasePrice = service.PurchasePrice;
            entities.SellingPrice = service.SellingPrice;
            entities.Unit = service.Unit;
            entities.QuantityInStock =
                CalculatorExtension.QuantityInPrinciple(request.Data.QuantityInStock, service.QuantityPrinciple.Value);

            serviceRepository.Add(entities);
        }

        return Result.Success();
    }
}
