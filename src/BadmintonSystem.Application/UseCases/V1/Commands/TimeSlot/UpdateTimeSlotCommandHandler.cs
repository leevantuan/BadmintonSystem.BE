using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.TimeSlot;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;

namespace BadmintonSystem.Application.UseCases.V1.Commands.TimeSlot;

public sealed class UpdateTimeSlotCommandHandler(
    IMapper mapper,
    IRepositoryBase<Domain.Entities.TimeSlot, Guid> timeSlotRepository)
    : ICommandHandler<Command.UpdateTimeSlotCommand, Response.TimeSlotResponse>
{
    public async Task<Result<Response.TimeSlotResponse>> Handle
        (Command.UpdateTimeSlotCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.TimeSlot timeSlot = await timeSlotRepository.FindByIdAsync(request.Data.Id, cancellationToken)
                                            ?? throw new TimeSlotException.TimeSlotNotFoundException(request.Data.Id);

        timeSlot.StartTime = request.Data.StartTime ?? timeSlot.StartTime;
        timeSlot.EndTime = request.Data.EndTime ?? timeSlot.EndTime;

        Response.TimeSlotResponse? result = mapper.Map<Response.TimeSlotResponse>(timeSlot);

        return Result.Success(result);
    }
}
