using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V2.Club;
using BadmintonSystem.Domain.Abstractions;
using BadmintonSystem.Domain.Abstractions.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V2.Club.Commands;
public sealed class CreateClubCommandHandler : ICommandHandler<Command.CreateClubCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IPublisher _publisher;
    private readonly IRepositoryBase<Domain.Entities.Club, Guid> _clubRepository;

    public CreateClubCommandHandler(IUnitOfWork unitOfWork,
                                      IMapper mapper,
                                      IPublisher publisher,
                                      IRepositoryBase<Domain.Entities.Club, Guid> clubRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _publisher = publisher;
        _clubRepository = clubRepository;
    }

    public async Task<Result> Handle(Command.CreateClubCommand request, CancellationToken cancellationToken)
    {
        var isNameExists = await _clubRepository.FindAll(x => x.Name.ToLower().Trim().Equals(request.Data.Name.ToLower().Trim())).ToListAsync();
        if (isNameExists.Any())
            return Result.Failure(new Error("200", "Name Exists"));

        var club = _mapper.Map<Domain.Entities.Club>(request.Data);

        _clubRepository.Add(club);

        //await _unitOfWork.SaveChangesAsync(cancellationToken);

        //await _publisher.Publish(new DomainEvent.ClubCreated(club.Id), cancellationToken);
        //await _publisher.Publish(new DomainEvent.ClubDeleted(club.Id), cancellationToken);

        await Task.WhenAll(
            _publisher.Publish(new DomainEvent.ClubCreated(club.Id), cancellationToken),
            _publisher.Publish(new DomainEvent.ClubDeleted(club.Id), cancellationToken));

        return Result.Success();
    }
}
