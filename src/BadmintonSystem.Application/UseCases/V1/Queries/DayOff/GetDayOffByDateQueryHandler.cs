using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.DayOff;
using BadmintonSystem.Domain.Abstractions.Repositories;

namespace BadmintonSystem.Application.UseCases.V1.Queries.DayOff;

public sealed class GetDayOffByDateQueryHandler(
    IMapper mapper,
    IRepositoryBase<Domain.Entities.DayOff, Guid> dayOffRepository)
    : IQueryHandler<Query.GetDayOffByDateQuery, Response.DayOffDetailResponse>
{
    public Task<Result<Response.DayOffDetailResponse>> Handle
        (Query.GetDayOffByDateQuery request, CancellationToken cancellationToken)
    {
        var result = new Response.DayOffDetailResponse();

        DateTime dateRequest = request.Date.Date;

        Domain.Entities.DayOff? dayOff = dayOffRepository.FindAll(x => x.Date.Date == dateRequest).FirstOrDefault();

        if (dayOff == null)
        {
            return Task.FromResult(Result.Success(result));
        }

        result = mapper.Map<Response.DayOffDetailResponse>(dayOff);

        return Task.FromResult(Result.Success(result));
    }
}
