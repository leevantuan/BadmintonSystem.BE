using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Club;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Club;

public sealed class DeleteClubsCommandHandler(
    IRepositoryBase<Domain.Entities.Club, Guid> clubRepository)
    : ICommandHandler<Command.DeleteClubsCommand>
{
    public async Task<Result> Handle(Command.DeleteClubsCommand request, CancellationToken cancellationToken)
    {
        List<Domain.Entities.Club> clubs = new();

        foreach (string id in request.Ids)
        {
            var idValue = Guid.Parse(id);

            Domain.Entities.Club club = await clubRepository.FindByIdAsync(idValue, cancellationToken)
                                        ?? throw new ClubException.ClubNotFoundException(idValue);

            clubs.Add(club);
        }

        clubRepository.RemoveMultiple(clubs);

        return Result.Success();
    }
}
