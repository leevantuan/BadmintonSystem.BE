using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.DayOff;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.DayOff;

public sealed class DeleteDayOffsCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.DayOff, Guid> dayOffRepository)
    : ICommandHandler<Command.DeleteDayOffsCommand>
{
    public async Task<Result> Handle(Command.DeleteDayOffsCommand request, CancellationToken cancellationToken)
    {
        List<Domain.Entities.DayOff> dayOffs = new();

        foreach (string id in request.Ids)
        {
            var idValue = Guid.Parse(id);

            Domain.Entities.DayOff dayOff = await dayOffRepository.FindByIdAsync(idValue, cancellationToken)
                                            ?? throw new DayOffException.DayOffNotFoundException(idValue);

            dayOffs.Add(dayOff);
        }

        dayOffRepository.RemoveMultiple(dayOffs);

        return Result.Success();
    }
}
