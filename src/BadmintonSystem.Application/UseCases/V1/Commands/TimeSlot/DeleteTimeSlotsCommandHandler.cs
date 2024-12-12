using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.TimeSlot;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.TimeSlot;

public sealed class DeleteTimeSlotsCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.TimeSlot, Guid> timeSlotRepository)
    : ICommandHandler<Command.DeleteTimeSlotsCommand>
{
    public async Task<Result> Handle(Command.DeleteTimeSlotsCommand request, CancellationToken cancellationToken)
    {
        List<Domain.Entities.TimeSlot> timeSlots = new();

        foreach (string id in request.Ids)
        {
            var idValue = Guid.Parse(id);

            Domain.Entities.TimeSlot timeSlot = await timeSlotRepository.FindByIdAsync(idValue, cancellationToken)
                                                ?? throw new TimeSlotException.TimeSlotNotFoundException(idValue);

            timeSlots.Add(timeSlot);
        }

        timeSlotRepository.RemoveMultiple(timeSlots);

        return Result.Success();
    }
}
