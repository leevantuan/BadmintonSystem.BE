using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.YardType;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.YardType;

public sealed class CreateYardTypeCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.YardType, Guid> yardTypeRepository)
    : ICommandHandler<Command.CreateYardTypeCommand, Response.YardTypeResponse>
{
    public Task<Result<Response.YardTypeResponse>> Handle
        (Command.CreateYardTypeCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.YardType yardType = mapper.Map<Domain.Entities.YardType>(request.Data);

        yardTypeRepository.Add(yardType);

        Response.YardTypeResponse? result = mapper.Map<Response.YardTypeResponse>(yardType);

        return Task.FromResult(Result.Success(result));
    }
}
