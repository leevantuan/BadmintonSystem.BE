using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V2.Category;
using BadmintonSystem.Domain.Abstractions.Dappers;

namespace BadmintonSystem.Application.UseCases.V2.Category.Queries;
public sealed class GetAllCategoryQueryHandler : IQueryHandler<Query.GetAllCategory, List<Response.CategoryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllCategoryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<List<Response.CategoryResponse>>> Handle(Query.GetAllCategory request, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.Categories.GetAllAsync();

        var result = _mapper.Map<List<Response.CategoryResponse>>(category);

        return result;
    }
}
