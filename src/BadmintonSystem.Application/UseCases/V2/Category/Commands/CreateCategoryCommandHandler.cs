using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V2.Category;
using BadmintonSystem.Domain.Abstractions;
using BadmintonSystem.Domain.Abstractions.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V2.Category.Commands;
public sealed class CreateCategoryCommandHandler : ICommandHandler<Command.CreateCategoryCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IPublisher _publisher;
    private readonly IRepositoryBase<Domain.Entities.Category, Guid> _categoryRepository;

    public CreateCategoryCommandHandler(IUnitOfWork unitOfWork,
                                      IMapper mapper,
                                      IPublisher publisher,
                                      IRepositoryBase<Domain.Entities.Category, Guid> categoryRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _publisher = publisher;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result> Handle(Command.CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var isNameExists = await _categoryRepository.FindAll(x => x.Name.ToLower().Trim().Equals(request.Data.Name.ToLower().Trim())).ToListAsync();
        if (isNameExists.Any())
            return Result.Failure(new Error("200", "Name Exists"));

        var category = _mapper.Map<Domain.Entities.Category>(request.Data);

        _categoryRepository.Add(category);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        //await _publisher.Publish(new DomainEvent.CategoryCreated(category.Id), cancellationToken);
        //await _publisher.Publish(new DomainEvent.CategoryDeleted(category.Id), cancellationToken);

        await Task.WhenAll(
            _publisher.Publish(new DomainEvent.CategoryCreated(category.Id), cancellationToken),
            _publisher.Publish(new DomainEvent.CategoryDeleted(category.Id), cancellationToken));

        return Result.Success();
    }
}
