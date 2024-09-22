using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V2.Club;
using BadmintonSystem.Domain.Abstractions;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V2.Club.Commands;
public sealed class UpdateClubCommandHandler : ICommandHandler<Command.UpdateClubCommand>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepositoryBase<Domain.Entities.Club, Guid> _clubRepository;

    public UpdateClubCommandHandler(IMapper mapper,
                                      IUnitOfWork unitOfWork,
                                      IRepositoryBase<Domain.Entities.Club, Guid> clubRepository)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _clubRepository = clubRepository;
    }

    public async Task<Result> Handle(Command.UpdateClubCommand request, CancellationToken cancellationToken)
    {
        var club = await _clubRepository.FindByIdAsync(request.Id) ??
            throw new ClubException.ClubNotFoundException(request.Id);

        var isNameExists = await _clubRepository.FindAll(x => x.Name.ToLower().Trim().Equals(request.Data.Name.ToLower().Trim())).ToListAsync();

        if (isNameExists.Any())
            return Result.Failure(new Error("200", "Is Name Exists!"));

        club.UpdateClub(request.Data.Name, request.Data.Code, request.Data.HotLine, request.Data.FacebookPageLink, request.Data.InstagramPageLink, request.Data.MapLink, request.Data.ImageLink, request.Data.OpeningTime, request.Data.ClosingTime);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
