using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Category;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Category;

public sealed class GetCategoryByIdQueryHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Category, Guid> categoryRepository)
    : IQueryHandler<Query.GetCategoryByIdQuery, Response.CategoryResponse>
{
    public async Task<Result<Response.CategoryResponse>> Handle
        (Query.GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        Domain.Entities.Category service = await categoryRepository.FindByIdAsync(request.Id)
                                           ?? throw new CategoryException.CategoryNotFoundException(request.Id);

        Response.CategoryResponse? result = mapper.Map<Response.CategoryResponse>(service);

        return Result.Success(result);
    }
}
