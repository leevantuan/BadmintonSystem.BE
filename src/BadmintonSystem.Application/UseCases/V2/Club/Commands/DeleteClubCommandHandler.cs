using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V2.Club;
using BadmintonSystem.Domain.Abstractions;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using MediatR;

namespace BadmintonSystem.Application.UseCases.V2.Club.Commands;
public sealed class DeleteClubCommandHandler : ICommandHandler<Command.DeleteClubCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepositoryBase<Domain.Entities.Club, Guid> _clubRepository;
    private readonly IPublisher _publisher; // Use Send Email of MediatR and Domain Event

    public DeleteClubCommandHandler(IUnitOfWork unitOfWork,
                                      IRepositoryBase<Domain.Entities.Club, Guid> clubRepository,
                                      IPublisher publisher)
    {
        _unitOfWork = unitOfWork;
        _clubRepository = clubRepository;
        _publisher = publisher;
    }

    public async Task<Result> Handle(Command.DeleteClubCommand request, CancellationToken cancellationToken)
    {
        // Find By Id
        var club = await _clubRepository.FindByIdAsync(request.Id) ??
            throw new ClubException.ClubNotFoundException(request.Id);

        _clubRepository.Remove(club);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _publisher.Publish(new DomainEvent.ClubDeleted(request.Id), cancellationToken);

        return Result.Success();
    }
}
