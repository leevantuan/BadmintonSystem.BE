using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V2.Category;
using BadmintonSystem.Domain.Abstractions;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V2.Category.Commands;
public sealed class UpdateCategoryCommandHandler : ICommandHandler<Command.UpdateCategoryCommand>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepositoryBase<Domain.Entities.Category, Guid> _categoryRepository;

    public UpdateCategoryCommandHandler(IMapper mapper,
                                      IUnitOfWork unitOfWork,
                                      IRepositoryBase<Domain.Entities.Category, Guid> categoryRepository)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result> Handle(Command.UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.FindByIdAsync(request.Id) ??
            throw new CategoryException.CategoryNotFoundException(request.Id);

        var isNameExists = await _categoryRepository.FindAll(x => x.Name.ToLower().Trim().Equals(request.Name.ToLower().Trim())).ToListAsync();

        if (isNameExists.Any())
            return Result.Failure(new Error("200", "Is Name Exists!"));

        category.UpdateCategory(request.Name);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
