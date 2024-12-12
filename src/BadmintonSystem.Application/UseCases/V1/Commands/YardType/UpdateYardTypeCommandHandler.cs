using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.YardType;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.YardType;

public sealed class UpdateYardTypeCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.YardType, Guid> yardTypeRepository)
    : ICommandHandler<Command.UpdateYardTypeCommand, Response.YardTypeResponse>
{
    public async Task<Result<Response.YardTypeResponse>> Handle
        (Command.UpdateYardTypeCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.YardType yardType = await yardTypeRepository.FindByIdAsync(request.Data.Id, cancellationToken)
                                            ?? throw new YardTypeException.YardTypeNotFoundException(request.Data.Id);

        yardType.Name = request.Data.Name ?? yardType.Name;
        yardType.Price = request.Data.Price;

        Response.YardTypeResponse? result = mapper.Map<Response.YardTypeResponse>(yardType);

        return Result.Success(result);
    }
}
