using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Yard;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Yard;

public sealed class CreateYardCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Yard, Guid> yardRepository)
    : ICommandHandler<Command.CreateYardCommand, Response.YardResponse>
{
    public Task<Result<Response.YardResponse>> Handle
        (Command.CreateYardCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Yard yard = mapper.Map<Domain.Entities.Yard>(request.Data);

        yard.IsStatus = (StatusEnum)request.Data.IsStatus;

        yardRepository.Add(yard);

        Response.YardResponse? result = mapper.Map<Response.YardResponse>(yard);

        return Task.FromResult(Result.Success(result));
    }
}
