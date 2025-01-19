using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Category;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Category;

public sealed class UpdateCategoryCommandHandler(
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Category, Guid> categoryRepository)
    : ICommandHandler<Command.UpdateCategoryCommand, Response.CategoryResponse>
{
    public async Task<Result<Response.CategoryResponse>> Handle
        (Command.UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Category category =
            await categoryRepository.FindByIdAsync(request.Data.Id.Value, cancellationToken)
            ?? throw new CategoryException.CategoryNotFoundException(request.Data.Id ??
                                                                     Guid.Empty);

        category.Name = request.Data.Name ?? category.Name;

        Response.CategoryResponse? result = mapper.Map<Response.CategoryResponse>(category);

        return Result.Success(result);
    }
}
