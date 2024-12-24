using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.FixedSchedule;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.FixedSchedule;

public sealed class UpdateFixedScheduleCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.FixedSchedule, Guid> fixedScheduleRepository)
    : ICommandHandler<Command.UpdateFixedScheduleCommand, Response.FixedScheduleResponse>
{
    public async Task<Result<Response.FixedScheduleResponse>> Handle
        (Command.UpdateFixedScheduleCommand request, CancellationToken cancellationToken)
    {
        _ = context.AppUsers.FirstOrDefault(x => x.Id == request.Data.UserId)
            ?? throw new IdentityException.AppUserNotFoundException(request.Data.UserId ?? Guid.Empty);

        Domain.Entities.FixedSchedule fixedSchedule =
            await fixedScheduleRepository.FindByIdAsync(request.Data.Id, cancellationToken)
            ?? throw new FixedScheduleException.FixedScheduleNotFoundException(request.Data.Id);

        fixedSchedule.UserId = request.Data.UserId ?? fixedSchedule.UserId;
        fixedSchedule.StartDate = request.Data.StartDate ?? fixedSchedule.StartDate;
        fixedSchedule.EndDate = request.Data.EndDate ?? fixedSchedule.EndDate;

        Response.FixedScheduleResponse? result = mapper.Map<Response.FixedScheduleResponse>(fixedSchedule);

        return Result.Success(result);
    }
}
