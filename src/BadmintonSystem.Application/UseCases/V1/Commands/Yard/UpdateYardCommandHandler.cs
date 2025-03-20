using AutoMapper;
using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Services;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Yard;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Domain.Exceptions;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Yard;

public sealed class UpdateYardCommandHandler(
    IMapper mapper,
    IRedisService redisService,
    ICurrentTenantService currentTenantService,
    IRepositoryBase<Domain.Entities.Yard, Guid> yardRepository)
    : ICommandHandler<Command.UpdateYardCommand, Response.YardResponse>
{
    public async Task<Result<Response.YardResponse>> Handle
        (Command.UpdateYardCommand request, CancellationToken cancellationToken)
    {
        string endpoint = $"BMTSYS_{currentTenantService.Code.ToString()}-get-yard-prices-by-date";

        await redisService.DeletesAsync(endpoint);

        Domain.Entities.Yard yard = await yardRepository.FindByIdAsync(request.Data.Id, cancellationToken)
                                    ?? throw new YardException.YardNotFoundException(request.Data.Id);

        if (request.Data.Name != null && request.Data.Name != yard.Name)
        {
            Domain.Entities.Yard? isNameExists =
                await yardRepository.FindSingleAsync(x => x.Name == request.Data.Name, cancellationToken);

            if (isNameExists != null)
            {
                return Result.Failure<Response.YardResponse>(new Error("400", "Name Exists!"));
            }
        }

        yard.Name = request.Data.Name ?? yard.Name;
        yard.YardTypeId = request.Data.YardTypeId;
        yard.IsStatus = (StatusEnum)request.Data.IsStatus;

        Response.YardResponse? result = mapper.Map<Response.YardResponse>(yard);

        return Result.Success(result);
    }
}
