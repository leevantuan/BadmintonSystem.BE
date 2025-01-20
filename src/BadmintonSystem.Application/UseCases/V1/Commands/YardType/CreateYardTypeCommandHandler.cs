using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.YardType;
using BadmintonSystem.Domain.Abstractions.Repositories;

namespace BadmintonSystem.Application.UseCases.V1.Commands.YardType;

public sealed class CreateYardTypeCommandHandler(
    IMapper mapper,
    IRepositoryBase<Domain.Entities.YardType, Guid> yardTypeRepository)
    : ICommandHandler<Command.CreateYardTypeCommand, Response.YardTypeResponse>
{
    public async Task<Result<Response.YardTypeResponse>> Handle
        (Command.CreateYardTypeCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.YardType? isNameExists =
            await yardTypeRepository.FindSingleAsync(x => x.Name == request.Data.Name, cancellationToken);

        if (isNameExists != null)
        {
            return Result.Failure<Response.YardTypeResponse>(new Error("400", "Name Exists!"));
        }

        Domain.Entities.YardType yardType = mapper.Map<Domain.Entities.YardType>(request.Data);

        yardTypeRepository.Add(yardType);

        Response.YardTypeResponse? result = mapper.Map<Response.YardTypeResponse>(yardType);

        return Result.Success(result);
    }
}
