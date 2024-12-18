using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Club;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Club;

public sealed class CreateClubCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Club, Guid> clubRepository)
    : ICommandHandler<Command.CreateClubCommand, Response.ClubResponse>
{
    public async Task<Result<Response.ClubResponse>> Handle
        (Command.CreateClubCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Club club = mapper.Map<Domain.Entities.Club>(request.Data);
        ClubInformation? clubInformation = mapper.Map<ClubInformation>(request.Data.ClubInformation);
        Address? clubAddress = mapper.Map<Address>(request.Data.ClubAddress);
        IEnumerable<ClubImage> clubImages =
            request.Data.ClubImages.Select(x => mapper.Map<ClubImage>(x));

        clubRepository.Add(club);
        context.Address.Add(clubAddress);

        await context.SaveChangesAsync(cancellationToken);

        context.ClubAddress.Add(new ClubAddress
        {
            AddressId = clubAddress.Id,
            ClubId = club.Id,
            Branch = "Branch"
        });

        clubInformation.ClubId = club.Id;
        context.ClubInformation.Add(clubInformation);

        foreach (ClubImage clubImage in clubImages)
        {
            clubImage.ClubId = club.Id;
            context.ClubImage.Add(clubImage);
        }

        Response.ClubResponse? result = mapper.Map<Response.ClubResponse>(club);

        return Result.Success(result);
    }
}
