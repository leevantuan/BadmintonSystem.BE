using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.FixedSchedule;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.FixedSchedule;

public sealed class CreateFixedScheduleCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.FixedSchedule, Guid> fixedScheduleRepository)
    : ICommandHandler<Command.CreateFixedScheduleCommand, Response.FixedScheduleResponse>
{
    public Task<Result<Response.FixedScheduleResponse>> Handle
        (Command.CreateFixedScheduleCommand request, CancellationToken cancellationToken)
    {
        _ = context.AppUsers.FirstOrDefault(x => x.Id == request.UserId)
            ?? throw new IdentityException.AppUserNotFoundException(request.UserId);

        Domain.Entities.FixedSchedule fixedSchedule = mapper.Map<Domain.Entities.FixedSchedule>(request.Data);

        fixedScheduleRepository.Add(fixedSchedule);

        Response.FixedScheduleResponse? result = mapper.Map<Response.FixedScheduleResponse>(fixedSchedule);

        return Task.FromResult(Result.Success(result));
    }
}
