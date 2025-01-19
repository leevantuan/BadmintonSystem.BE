using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.TimeSlot;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;

namespace BadmintonSystem.Application.UseCases.V1.Queries.TimeSlot;

public sealed class GetTimeSlotByIdQueryHandler(
    IMapper mapper,
    IRepositoryBase<Domain.Entities.TimeSlot, Guid> timeSlotRepository)
    : IQueryHandler<Query.GetTimeSlotByIdQuery, Response.TimeSlotResponse>
{
    public async Task<Result<Response.TimeSlotResponse>> Handle
        (Query.GetTimeSlotByIdQuery request, CancellationToken cancellationToken)
    {
        Domain.Entities.TimeSlot timeSlot = await timeSlotRepository.FindByIdAsync(request.Id, cancellationToken)
                                            ?? throw new TimeSlotException.TimeSlotNotFoundException(request.Id);

        Response.TimeSlotResponse? result = mapper.Map<Response.TimeSlotResponse>(timeSlot);

        return Result.Success(result);
    }
}
