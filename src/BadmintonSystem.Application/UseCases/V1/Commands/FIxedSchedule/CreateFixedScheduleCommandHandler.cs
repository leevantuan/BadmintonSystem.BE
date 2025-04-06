using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.FixedSchedule;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence;
using DayOfWeek = BadmintonSystem.Domain.Entities.DayOfWeek;
using Request = BadmintonSystem.Contract.Services.V1.FixedSchedule.Request;

namespace BadmintonSystem.Application.UseCases.V1.Commands.FixedSchedule;

public sealed class CreateFixedScheduleCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.FixedSchedule, Guid> fixedScheduleRepository)
    : ICommandHandler<Command.CreateFixedScheduleCommand, Response.FixedScheduleResponse>
{
    public async Task<Result<Response.FixedScheduleResponse>> Handle
        (Command.CreateFixedScheduleCommand request, CancellationToken cancellationToken)
    {
        //_ = await context.AppUsers.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken)
        //    ?? throw new IdentityException.AppUserNotFoundException(request.UserId);

        Domain.Entities.FixedSchedule fixedSchedule = mapper.Map<Domain.Entities.FixedSchedule>(request.Data);

        fixedScheduleRepository.Add(fixedSchedule);
        await context.SaveChangesAsync(cancellationToken);

        await CreateDayOfWeek(request.Data.DayOfWeeks, fixedSchedule.Id, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        Response.FixedScheduleResponse? result = mapper.Map<Response.FixedScheduleResponse>(fixedSchedule);

        return Result.Success(result);
    }

    private async Task CreateDayOfWeek
    (List<Request.CreateDayOfWeekDetailRequest> dayOfWeekRequests, Guid fixedScheduleId,
        CancellationToken cancellationToken)
    {
        foreach (Request.CreateDayOfWeekDetailRequest dayOfWeekRequest in dayOfWeekRequests)
        {
            var dayOfWeek = new DayOfWeek
            {
                Id = Guid.NewGuid(),
                WeekName = dayOfWeekRequest.WeekName,
                FixedScheduleId = fixedScheduleId
            };

            context.DayOfWeek.Add(dayOfWeek);
            await context.SaveChangesAsync(cancellationToken);

            CreateTimeSlot(dayOfWeekRequest.TimeSlotIds, dayOfWeek.Id);
        }
    }

    private void CreateTimeSlot(List<Guid> timeSlotIds, Guid dayOfWeekId)
    {
        context.TimeSlotOfWeek.AddRange(timeSlotIds.Select(x => new TimeSlotOfWeek
        {
            TimeSlotId = x,
            DayOfWeekId = dayOfWeekId
        }));
    }
}
