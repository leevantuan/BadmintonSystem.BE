using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Club;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Club;

public sealed class UpdateClubCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Club, Guid> clubRepository)
    : ICommandHandler<Command.UpdateClubCommand, Response.ClubResponse>
{
    public async Task<Result<Response.ClubResponse>> Handle
        (Command.UpdateClubCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Club club = await clubRepository.FindByIdAsync(request.Data.Id, cancellationToken)
                                    ?? throw new ClubException.ClubNotFoundException(request.Data.Id);

        club.Name = request.Data.Name ?? club.Name;
        club.Hotline = request.Data.Hotline ?? club.Hotline;
        club.OpeningTime = request.Data.OpeningTime ?? club.OpeningTime;
        club.ClosingTime = request.Data.ClosingTime ?? club.ClosingTime;
        club.Code = request.Data.Code ?? club.Code;

        Response.ClubResponse? result = mapper.Map<Response.ClubResponse>(club);

        return Result.Success(result);
    }
}
