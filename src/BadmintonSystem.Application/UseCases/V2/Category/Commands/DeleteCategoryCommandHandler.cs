using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V2.Category;
using BadmintonSystem.Domain.Abstractions;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using MediatR;

namespace BadmintonSystem.Application.UseCases.V2.Category.Commands;
public sealed class DeleteCategoryCommandHandler : ICommandHandler<Command.DeleteCategoryCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepositoryBase<Domain.Entities.Category, Guid> _categoryRepository;
    private readonly IPublisher _publisher; // Use Send Email of MediatR and Domain Event

    public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork,
                                      IRepositoryBase<Domain.Entities.Category, Guid> categoryRepository,
                                      IPublisher publisher)
    {
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
        _publisher = publisher;
    }

    public async Task<Result> Handle(Command.DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        // Find By Id
        var category = await _categoryRepository.FindByIdAsync(request.Id) ??
            throw new CategoryException.CategoryNotFoundException(request.Id);

        _categoryRepository.Remove(category);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _publisher.Publish(new DomainEvent.CategoryDeleted(request.Id), cancellationToken);

        return Result.Success();
    }
}
