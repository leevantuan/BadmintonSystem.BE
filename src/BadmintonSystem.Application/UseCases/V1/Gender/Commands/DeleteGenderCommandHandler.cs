using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Gender;
using BadmintonSystem.Domain.Abstractions;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using MediatR;

namespace BadmintonSystem.Application.UseCases.V1.Gender.Commands;
public sealed class DeleteGenderCommandHandler : ICommandHandler<Command.DeleteGenderCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepositoryBase<Domain.Entities.Gender, Guid> _genderRepository;
    private readonly IPublisher _publisher; // Use Send Email of MediatR and Domain Event

    public DeleteGenderCommandHandler(IUnitOfWork unitOfWork,
                                      IRepositoryBase<Domain.Entities.Gender, Guid> genderRepository,
                                      IPublisher publisher)
    {
        _unitOfWork = unitOfWork;
        _genderRepository = genderRepository;
        _publisher = publisher;
    }

    public async Task<Result> Handle(Command.DeleteGenderCommand request, CancellationToken cancellationToken)
    {
        // Find By Id
        var gender = await _genderRepository.FindByIdAsync(request.Id) ??
            throw new GenderException.GenderNotFoundException(request.Id);

        _genderRepository.Remove(gender);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // ==> Notification Send Mail Created
        // Should asynchronous
        await _publisher.Publish(new DomainEvent.GenderDeleted(request.Id), cancellationToken);

        return Result.Success();
    }
}
