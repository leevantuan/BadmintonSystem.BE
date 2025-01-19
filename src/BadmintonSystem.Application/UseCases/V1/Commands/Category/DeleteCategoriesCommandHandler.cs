using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Category;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Category;

public sealed class DeleteCategoriesCommandHandler(
    IRepositoryBase<Domain.Entities.Category, Guid> categoryRepository)
    : ICommandHandler<Command.DeleteCategoriesCommand>
{
    public async Task<Result> Handle(Command.DeleteCategoriesCommand request, CancellationToken cancellationToken)
    {
        List<Domain.Entities.Category> categories = new();

        foreach (string id in request.Ids)
        {
            var idValue = Guid.Parse(id);

            Domain.Entities.Category category = await categoryRepository.FindByIdAsync(idValue, cancellationToken)
                                                ?? throw new CategoryException.CategoryNotFoundException(idValue);

            categories.Add(category);
        }

        categoryRepository.RemoveMultiple(categories);

        return Result.Success();
    }
}
