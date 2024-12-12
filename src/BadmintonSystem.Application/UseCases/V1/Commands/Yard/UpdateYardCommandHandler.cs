using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Yard;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Yard;

public sealed class UpdateYardCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Yard, Guid> yardRepository)
    : ICommandHandler<Command.UpdateYardCommand, Response.YardResponse>
{
    public async Task<Result<Response.YardResponse>> Handle
        (Command.UpdateYardCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Yard yard = await yardRepository.FindByIdAsync(request.Data.Id, cancellationToken)
                                    ?? throw new YardException.YardNotFoundException(request.Data.Id);

        yard.Name = request.Data.Name ?? yard.Name;
        yard.YardTypeId = request.Data.YardTypeId;

        Response.YardResponse? result = mapper.Map<Response.YardResponse>(yard);

        return Result.Success(result);
    }
}
