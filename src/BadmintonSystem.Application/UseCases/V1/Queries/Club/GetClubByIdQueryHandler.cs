using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Club;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Club;

public sealed class GetClubByIdQueryHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Club, Guid> clubRepository)
    : IQueryHandler<Query.GetClubByIdQuery, Response.ClubResponse>
{
    public async Task<Result<Response.ClubResponse>> Handle
        (Query.GetClubByIdQuery request, CancellationToken cancellationToken)
    {
        Domain.Entities.Club club = await clubRepository.FindByIdAsync(request.Id, cancellationToken)
                                    ?? throw new ClubException.ClubNotFoundException(request.Id);

        Response.ClubResponse? result = mapper.Map<Response.ClubResponse>(club);

        return Result.Success(result);
    }
}
