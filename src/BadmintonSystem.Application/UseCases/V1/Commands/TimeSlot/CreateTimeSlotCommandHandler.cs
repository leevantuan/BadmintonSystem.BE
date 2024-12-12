using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.TimeSlot;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.TimeSlot;

public sealed class CreateTimeSlotCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.TimeSlot, Guid> timeSlotRepository)
    : ICommandHandler<Command.CreateTimeSlotCommand, Response.TimeSlotResponse>
{
    public Task<Result<Response.TimeSlotResponse>> Handle
        (Command.CreateTimeSlotCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.TimeSlot timeSlot = mapper.Map<Domain.Entities.TimeSlot>(request.Data);

        timeSlotRepository.Add(timeSlot);

        Response.TimeSlotResponse? result = mapper.Map<Response.TimeSlotResponse>(timeSlot);

        return Task.FromResult(Result.Success(result));
    }
}
