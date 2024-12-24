using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.DayOff;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.DayOff;

public sealed class UpdateDayOffCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.DayOff, Guid> dayOffRepository)
    : ICommandHandler<Command.UpdateDayOffCommand, Response.DayOffResponse>
{
    public async Task<Result<Response.DayOffResponse>> Handle
        (Command.UpdateDayOffCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.DayOff dayOff =
            await dayOffRepository.FindByIdAsync(request.Data.Id, cancellationToken)
            ?? throw new DayOffException.DayOffNotFoundException(request.Data.Id);

        dayOff.Date = request.Data.Date ?? dayOff.Date;
        dayOff.Content = request.Data.Content ?? dayOff.Content;

        Response.DayOffResponse? result = mapper.Map<Response.DayOffResponse>(dayOff);

        return Result.Success(result);
    }
}
