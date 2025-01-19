using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Category;
using BadmintonSystem.Domain.Abstractions.Repositories;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Category;

public sealed class CreateCategoryCommandHandler(
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Category, Guid> categoryRepository)
    : ICommandHandler<Command.CreateCategoryCommand, Response.CategoryResponse>
{
    public Task<Result<Response.CategoryResponse>> Handle
        (Command.CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Category category = mapper.Map<Domain.Entities.Category>(request.Data);

        categoryRepository.Add(category);

        Response.CategoryResponse? result = mapper.Map<Response.CategoryResponse>(category);

        return Task.FromResult(Result.Success(result));
    }
}
