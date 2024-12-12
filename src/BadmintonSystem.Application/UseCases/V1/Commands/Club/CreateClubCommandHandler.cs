using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Club;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Club;

public sealed class CreateClubCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Club, Guid> clubRepository)
    : ICommandHandler<Command.CreateClubCommand, Response.ClubResponse>
{
    public Task<Result<Response.ClubResponse>> Handle
        (Command.CreateClubCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Club club = mapper.Map<Domain.Entities.Club>(request.Data);

        clubRepository.Add(club);

        Response.ClubResponse? result = mapper.Map<Response.ClubResponse>(club);

        return Task.FromResult(Result.Success(result));
    }
}
