using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Club;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Entities;
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
        Domain.Entities.Club club = await clubRepository.FindByIdAsync(request.Data.Id, cancellationToken);

        ClubInformation? clubInformation =
            context.ClubInformation.FirstOrDefault(x => x.ClubId == club.Id);

        ClubAddress? clubAddress =
            context.ClubAddress.FirstOrDefault(x => x.ClubId == request.Data.ClubInformation.ClubId);

        Address? address = context.Address.FirstOrDefault(x => x.Id == clubAddress.AddressId);

        IEnumerable<ClubImage> clubImages =
            context.ClubImage.Where(x => x.ClubId == club.Id).ToList();

        IEnumerable<ClubImage> clubImagesRequest =
            request.Data.ClubImages.Select(x => mapper.Map<ClubImage>(x));

        // Club information
        clubInformation.InstagramLink = request.Data.ClubInformation.InstagramLink;
        clubInformation.FacebookPageLink = request.Data.ClubInformation.FacebookPageLink;
        clubInformation.MapLink = request.Data.ClubInformation.MapLink;

        // Club Images
        context.ClubImage.RemoveRange(clubImages);

        foreach (ClubImage clubImage in clubImagesRequest)
        {
            clubImage.ClubId = club.Id;
            context.ClubImage.Add(clubImage);
        }

        // Address Club
        await context.SaveChangesAsync(cancellationToken);

        context.ClubAddress.Update(new ClubAddress
        {
            AddressId = request.Data.ClubAddress.Id,
            ClubId = request.Data.Id,
            Branch = "Branch"
        });

        // Address
        address.Unit = request.Data.ClubAddress.Unit;
        address.Street = request.Data.ClubAddress.Street;
        address.AddressLine1 = request.Data.ClubAddress.AddressLine1;
        address.AddressLine2 = request.Data.ClubAddress.AddressLine2;
        address.City = request.Data.ClubAddress.City;
        address.Province = request.Data.ClubAddress.Province;

        Response.ClubResponse? result = mapper.Map<Response.ClubResponse>(club);

        return Result.Success(result);
    }
}
