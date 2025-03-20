using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Services;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.YardType;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;

namespace BadmintonSystem.Application.UseCases.V1.Commands.YardType;

public sealed class DeleteYardTypesCommandHandler(
    IRedisService redisService,
    ICurrentTenantService currentTenantService,
    IRepositoryBase<Domain.Entities.YardType, Guid> yardTypeRepository)
    : ICommandHandler<Command.DeleteYardTypesCommand>
{
    public async Task<Result> Handle(Command.DeleteYardTypesCommand request, CancellationToken cancellationToken)
    {
        string endpoint = $"BMTSYS_{currentTenantService.Code.ToString()}-get-yard-prices-by-date";

        await redisService.DeletesAsync(endpoint);

        List<Domain.Entities.YardType> yardTypes = new();

        foreach (string id in request.Ids)
        {
            var idValue = Guid.Parse(id);

            Domain.Entities.YardType yardType = await yardTypeRepository.FindByIdAsync(idValue, cancellationToken)
                                                ?? throw new YardTypeException.YardTypeNotFoundException(idValue);

            yardTypes.Add(yardType);
        }

        yardTypeRepository.RemoveMultiple(yardTypes);

        return Result.Success();
    }
}
