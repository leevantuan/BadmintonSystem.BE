using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.FixedSchedule;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;

namespace BadmintonSystem.Application.UseCases.V1.Commands.FixedSchedule;

public sealed class DeleteFixedSchedulesCommandHandler(
    IRepositoryBase<Domain.Entities.FixedSchedule, Guid> fixedScheduleRepository)
    : ICommandHandler<Command.DeleteFixedSchedulesCommand>
{
    public async Task<Result> Handle(Command.DeleteFixedSchedulesCommand request, CancellationToken cancellationToken)
    {
        List<Domain.Entities.FixedSchedule> fixedSchedules = new();

        foreach (string id in request.Ids)
        {
            var idValue = Guid.Parse(id);

            Domain.Entities.FixedSchedule fixedSchedule =
                await fixedScheduleRepository.FindByIdAsync(idValue, cancellationToken)
                ?? throw new FixedScheduleException.FixedScheduleNotFoundException(idValue);

            fixedSchedules.Add(fixedSchedule);
        }

        fixedScheduleRepository.RemoveMultiple(fixedSchedules);

        return Result.Success();
    }
}
