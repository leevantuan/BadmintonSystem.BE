using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V2.Category;
using BadmintonSystem.Domain.Abstractions.Dappers;

namespace BadmintonSystem.Application.UseCases.V2.Category.Queries;
public sealed class GetByIdCategoryQueryHandler : IQueryHandler<Query.GetCategoryByIdQuery, Response.CategoryResponse>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetByIdCategoryQueryHandler(IMapper mapper,
                                    IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Response.CategoryResponse>> Handle(Query.GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(request.Id);

        var result = _mapper.Map<Response.CategoryResponse>(category);

        return result;
    }
}
