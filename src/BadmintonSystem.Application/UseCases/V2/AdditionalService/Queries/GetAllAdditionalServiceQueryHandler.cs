using System.Linq.Expressions;
using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Enumerations;
using BadmintonSystem.Contract.Services.V2.AdditionalService;
using BadmintonSystem.Domain.Abstractions.Dappers;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V2.AdditionalService.Queries;
public sealed class GetAllAdditionalServiceQueryHandler : IQueryHandler<Query.GetAllAdditionalService, PagedResult<Response.AdditionalServiceResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    private readonly IRepositoryBase<Domain.Entities.AdditionalService, Guid> _additionalServiceRepository;

    public GetAllAdditionalServiceQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ApplicationDbContext context, IRepositoryBase<Domain.Entities.AdditionalService, Guid> additionalServiceRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _context = context;
        _additionalServiceRepository = additionalServiceRepository;
    }

    public async Task<Result<PagedResult<Response.AdditionalServiceResponse>>> Handle(Query.GetAllAdditionalService request, CancellationToken cancellationToken)
    {
        if (request.SortColumnAndOrder.Any())
        {
            var PageIndex = request.PageIndex <= 0 ? PagedResult<Domain.Entities.AdditionalService>.DefaultPageIndex : request.PageIndex;
            var PageSize = request.PageSize <= 0 ? PagedResult<Domain.Entities.AdditionalService>.DefaultPageSize
                : request.PageSize > PagedResult<Domain.Entities.AdditionalService>.DefaultPageSize
                ? PagedResult<Domain.Entities.AdditionalService>.UpperPageSize
                : request.PageSize;

            var additionalServicesQuery = string.IsNullOrWhiteSpace(request.SearchTerm)
                ? @$"SELECT a.*, c.Name AS CategoryName, cl.Name AS ClubName
                     FROM {nameof(Domain.Entities.AdditionalService)} a 
                     INNER JOIN {nameof(Domain.Entities.Category)} c ON a.CategoryId = c.Id 
                     INNER JOIN {nameof(Domain.Entities.Club)} cl ON a.ClubId = cl.Id 
                     ORDER BY "
                : $@"SELECT a.*, c.Name, cl.Name 
                     FROM {nameof(Domain.Entities.AdditionalService)} a 
                     INNER JOIN {nameof(Domain.Entities.Category)} c ON a.CategoryId = c.Id 
                     INNER JOIN {nameof(Domain.Entities.Club)} cl ON a.ClubId = cl.Id 
                     WHERE a.{nameof(Domain.Entities.AdditionalService.Name)} LIKE '%{request.SearchTerm}%' 
                     ORDER BY ";

            foreach (var item in request.SortColumnAndOrder)
                additionalServicesQuery += item.Value == SortOrder.Descending
                    ? $"a.{item.Key} DESC, "
                    : $"a.{item.Key} ASC, ";

            additionalServicesQuery = additionalServicesQuery.Remove(additionalServicesQuery.Length - 2);

            additionalServicesQuery += $" OFFSET {(PageIndex - 1) * PageSize} ROWS FETCH NEXT {PageSize} ROWS ONLY";

            var additionalServices = await _context.AdditionalServices.FromSqlRaw(additionalServicesQuery)
                .ToListAsync(cancellationToken: cancellationToken);

            var totalCount = await _context.AdditionalServices.CountAsync(cancellationToken);

            var additionalServicePagedResult = PagedResult<Domain.Entities.AdditionalService>.Create(additionalServices,
                PageIndex,
                PageSize,
                totalCount);

            var result = _mapper.Map<PagedResult<Response.AdditionalServiceResponse>>(additionalServicePagedResult);

            return Result.Success(result);
        }
        else // =>> Entity Framework
        {
            var additionalServicesQuery = string.IsNullOrWhiteSpace(request.SearchTerm)
            ? _additionalServiceRepository.FindAll()
            : _additionalServiceRepository.FindAll(x => x.Name.Contains(request.SearchTerm));


            additionalServicesQuery = request.SortOrder == SortOrder.Descending
            ? additionalServicesQuery.OrderByDescending(GetSortProperty(request))
            : additionalServicesQuery.OrderBy(GetSortProperty(request));

            var genders = await PagedResult<Domain.Entities.AdditionalService>.CreateAsync(additionalServicesQuery,
                request.PageIndex,
                request.PageSize);

            var result = _mapper.Map<PagedResult<Response.AdditionalServiceResponse>>(genders);
            return Result.Success(result);
        }
    }

    private static Expression<Func<Domain.Entities.AdditionalService, object>> GetSortProperty(Query.GetAllAdditionalService request)
         => request.SortColumn?.ToLower() switch
         {
             "name" => additionalService => additionalService.Name,
             "price" => additionalService => additionalService.Price, // Example fileds
             //"description" => additionalService => additionalService.Description, // Example fileds
             //_ => additionalService => additionalService.Id // Default, If Sort Colunm == null
             _ => additionalService => additionalService.DateCreated // Default Sort Descending on CreatedDate column
         };
}
