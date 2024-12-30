using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.FixedSchedule;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Domain.Exceptions;
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
        _ = context.AppUsers.FirstOrDefault(x => x.Id == request.UserId)
            ?? throw new IdentityException.AppUserNotFoundException(request.UserId);

        Domain.Entities.FixedSchedule fixedSchedule = mapper.Map<Domain.Entities.FixedSchedule>(request.Data);

        fixedScheduleRepository.Add(fixedSchedule);

        await context.SaveChangesAsync(cancellationToken);

        var dayOfWeeks = new List<DayOfWeek>();

        var timeSlotOfWeeks = new List<TimeSlotOfWeek>();

        // Day of week
        foreach (Request.CreateDayOfWeekDetailRequest dayOfWeekRequest in request.Data.DayOfWeeks)
        {
            var dayOfWeek = new Contract.Services.V1.DayOfWeek.Request.CreateDayOfWeekRequest
            {
                WeekName = dayOfWeekRequest.WeekName,
                FixedScheduleId = fixedSchedule.Id
            };

            DayOfWeek? dayOfWeekResult = mapper.Map<DayOfWeek>(dayOfWeek);

            dayOfWeeks.Add(dayOfWeekResult);

            foreach (Guid timeSlotId in dayOfWeekRequest.TimeSlotIds)
            {
                var timeSlot = new Contract.Services.V1.TimeSlotOfWeek.Request.CreateTimeSlotOfWeekRequest
                {
                    TimeSlotId = timeSlotId,
                    TimeSlotOfWeekId = dayOfWeekResult.Id
                };

                TimeSlotOfWeek? timeSlotResult = mapper.Map<TimeSlotOfWeek>(timeSlot);

                timeSlotOfWeeks.Add(timeSlotResult);
            }
        }

        Response.FixedScheduleResponse? result = mapper.Map<Response.FixedScheduleResponse>(fixedSchedule);

        return Result.Success(result);
    }
}
