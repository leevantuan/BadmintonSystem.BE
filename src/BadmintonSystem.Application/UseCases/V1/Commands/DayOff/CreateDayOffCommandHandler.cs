using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.DayOff;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.DayOff;

public sealed class CreateDayOffCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.DayOff, Guid> dayOffRepository)
    : ICommandHandler<Command.CreateDayOffCommand, Response.DayOffResponse>
{
    public Task<Result<Response.DayOffResponse>> Handle
        (Command.CreateDayOffCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.DayOff dayOff = mapper.Map<Domain.Entities.DayOff>(request.Data);

        dayOffRepository.Add(dayOff);

        Response.DayOffResponse? result = mapper.Map<Response.DayOffResponse>(dayOff);

        return Task.FromResult(Result.Success(result));
    }
}
