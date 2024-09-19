using System.Linq.Expressions;
using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Enumerations;
using BadmintonSystem.Contract.Services.V2.Category;
using BadmintonSystem.Domain.Abstractions.Dappers;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V2.Category.Queries;
public sealed class GetAllCategoryQueryHandler : IQueryHandler<Query.GetAllCategory, PagedResult<Response.CategoryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    private readonly IRepositoryBase<Domain.Entities.Category, Guid> _categoryRepository;

    public GetAllCategoryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ApplicationDbContext context, IRepositoryBase<Domain.Entities.Category, Guid> categoryRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _context = context;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<PagedResult<Response.CategoryResponse>>> Handle(Query.GetAllCategory request, CancellationToken cancellationToken)
    {
        if (request.SortColumnAndOrder.Any())
        {
            var PageIndex = request.PageIndex <= 0 ? PagedResult<Domain.Entities.Category>.DefaultPageIndex : request.PageIndex;
            var PageSize = request.PageSize <= 0 ? PagedResult<Domain.Entities.Category>.DefaultPageSize
                : request.PageSize > PagedResult<Domain.Entities.Category>.DefaultPageSize
                ? PagedResult<Domain.Entities.Category>.UpperPageSize
                : request.PageSize;

            var categoriesQuery = string.IsNullOrWhiteSpace(request.SearchTerm)
                ? @$"SELECT * FROM {nameof(Domain.Entities.Category)} ORDER BY "
                : $@"SELECT * FROM {nameof(Domain.Entities.Category)}
                              WHERE {nameof(Domain.Entities.Category.Name)} LIKE '%{request.SearchTerm}%' ORDER BY ";

            foreach (var item in request.SortColumnAndOrder)
                categoriesQuery += item.Value == SortOrder.Descending
                    ? $"{item.Key} DESC, "
                    : $"{item.Key} ASC, ";

            categoriesQuery = categoriesQuery.Remove(categoriesQuery.Length - 2);

            categoriesQuery += $" OFFSET {(PageIndex - 1) * PageSize} ROWS FETCH NEXT {PageSize} ROWS ONLY";

            var categorys = await _context.Categories.FromSqlRaw(categoriesQuery)
                .ToListAsync(cancellationToken: cancellationToken);

            var totalCount = await _context.Categories.CountAsync(cancellationToken);

            var categoryPagedResult = PagedResult<Domain.Entities.Category>.Create(categorys,
                PageIndex,
                PageSize,
                totalCount);

            var result = _mapper.Map<PagedResult<Response.CategoryResponse>>(categoryPagedResult);

            return Result.Success(result);
        }
        else // =>> Entity Framework
        {
            var categoriesQuery = string.IsNullOrWhiteSpace(request.SearchTerm)
            ? _categoryRepository.FindAll()
            : _categoryRepository.FindAll(x => x.Name.Contains(request.SearchTerm));


            categoriesQuery = request.SortOrder == SortOrder.Descending
            ? categoriesQuery.OrderByDescending(GetSortProperty(request))
            : categoriesQuery.OrderBy(GetSortProperty(request));

            var genders = await PagedResult<Domain.Entities.Category>.CreateAsync(categoriesQuery,
                request.PageIndex,
                request.PageSize);

            var result = _mapper.Map<PagedResult<Response.CategoryResponse>>(genders);
            return Result.Success(result);
        }
    }

    private static Expression<Func<Domain.Entities.Category, object>> GetSortProperty(Query.GetAllCategory request)
         => request.SortColumn?.ToLower() switch
         {
             "name" => category => category.Name,
             //"price" => category => category.Price, // Example fileds
             //"description" => category => category.Description, // Example fileds
             _ => category => category.Id // Default, If Sort Colunm == null
             //_ => category => category.CreatedDate // Default Sort Descending on CreatedDate column
         };
}
