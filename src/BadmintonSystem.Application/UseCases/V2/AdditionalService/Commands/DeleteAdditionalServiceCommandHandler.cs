using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V2.AdditionalService;
using BadmintonSystem.Domain.Abstractions;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using MediatR;

namespace BadmintonSystem.Application.UseCases.V2.AdditionalService.Commands;
public sealed class DeleteAdditionalServiceCommandHandler : ICommandHandler<Command.DeleteAdditionalServiceCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepositoryBase<Domain.Entities.AdditionalService, Guid> _additionalServiceRepository;
    private readonly IPublisher _publisher; // Use Send Email of MediatR and Domain Event

    public DeleteAdditionalServiceCommandHandler(IUnitOfWork unitOfWork,
                                      IRepositoryBase<Domain.Entities.AdditionalService, Guid> additionalServiceRepository,
                                      IPublisher publisher)
    {
        _unitOfWork = unitOfWork;
        _additionalServiceRepository = additionalServiceRepository;
        _publisher = publisher;
    }

    public async Task<Result> Handle(Command.DeleteAdditionalServiceCommand request, CancellationToken cancellationToken)
    {
        var additionalService = await _additionalServiceRepository.FindByIdAsync(request.Id) ??
            throw new AdditionalServiceException.AdditionalServiceNotFoundException(request.Id);

        _additionalServiceRepository.Remove(additionalService);

        //await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _publisher.Publish(new DomainEvent.AdditionalServiceDeleted(request.Id), cancellationToken);

        return Result.Success();
    }
}
