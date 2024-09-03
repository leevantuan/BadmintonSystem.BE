using System.Linq.Expressions;
using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Enumerations;
using BadmintonSystem.Contract.Services.Gender;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries;
public sealed class GetAllGenderQueryHandler : IQueryHandler<Query.GetAllGender, PagedResult<Response.GenderResponse>>
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    private readonly IRepositoryBase<Domain.Entities.Gender, Guid> _genderRepository;

    public GetAllGenderQueryHandler(IMapper mapper,
                                    ApplicationDbContext context,
                                    IRepositoryBase<Domain.Entities.Gender, Guid> genderRepository)
    {
        _mapper = mapper;
        _context = context;
        _genderRepository = genderRepository;
    }

    public async Task<Result<PagedResult<Response.GenderResponse>>> Handle(Query.GetAllGender request, CancellationToken cancellationToken)
    {
        // Loop, If SortColumnAndOrder have value then start if ==>
        // Case 1: If SortOrder and SortColumnAndOrder then take SortColumnAndOrder
        // Case 2: If SortColumnAndOrder not value and SortOrder have value
        if (request.SortColumnAndOrder.Any()) // =>>  Raw Query when order by multi column
        {
            // ======================== HANDLE PAGINATION INCOMING DATA =====================
            // CASE Handle then incoming data
            var PageIndex = request.PageIndex <= 0 ? PagedResult<Domain.Entities.Gender>.DefaultPageIndex : request.PageIndex;
            var PageSize = request.PageSize <= 0
                ? PagedResult<Domain.Entities.Gender>.DefaultPageSize
                : request.PageSize > PagedResult<Domain.Entities.Gender>.UpperPageSize
                ? PagedResult<Domain.Entities.Gender>.UpperPageSize : request.PageSize;

            // ============================================
            //var gendersQuery = string.IsNullOrWhiteSpace(request.SearchTerm)
            //    ? @$"SELECT * FROM {nameof(Domain.Entities.Gender)} ORDER BY "
            //    : @$"SELECT * FROM {nameof(Domain.Entities.Gender)}
            //            WHERE {nameof(Domain.Entities.Gender.Name)} LIKE '%{request.SearchTerm}%'
            //            OR {nameof(Domain.Entities.Gender.Description)} LIKE '%{request.SearchTerm}%'
            //            ORDER BY ";

            // ================================= SEARCH ================================
            // If SearchTerm != null
            // Run command WHERE ... LIKE
            // Also SELECT * FROM ...
            // ORDER BY ... ==> Continue
            var gendersQuery = string.IsNullOrWhiteSpace(request.SearchTerm)
                ? @$"SELECT * FROM {nameof(Domain.Entities.Gender)} ORDER BY "
                : @$"SELECT * FROM {nameof(Domain.Entities.Gender)}
                        WHERE {nameof(Domain.Entities.Gender.Name)} LIKE '%{request.SearchTerm}%'
                        ORDER BY ";

            // Loop in SortColumnAndOrder
            // If Value for item == Descending then Descending also Ascending
            foreach (var item in request.SortColumnAndOrder)
                gendersQuery += item.Value == SortOrder.Descending
                    ? $"{item.Key} DESC, "
                    : $"{item.Key} ASC, ";

            // Remove 2 character end for RAW SQL == ", "
            gendersQuery = gendersQuery.Remove(gendersQuery.Length - 2);

            // ========================= PAGINATION ===================
            // OFFSET Row before, FETCH NEXT Row tiếp theo
            gendersQuery += $" OFFSET {(PageIndex - 1) * PageSize} ROWS FETCH NEXT {PageSize} ROWS ONLY";

            // Run RAW Query SQL ==>
            var genders = await _context.Genders.FromSqlRaw(gendersQuery)
                .ToListAsync(cancellationToken: cancellationToken);

            var totalCount = await _context.Genders.CountAsync(cancellationToken);

            var genderPagedResult = PagedResult<Domain.Entities.Gender>.Create(genders,
                PageIndex,
                PageSize,
                totalCount);

            var result = _mapper.Map<PagedResult<Response.GenderResponse>>(genderPagedResult);

            return Result.Success(result);
        }
        else // =>> Entity Framework
        {
            var gendersQuery = string.IsNullOrWhiteSpace(request.SearchTerm)
            ? _genderRepository.FindAll()
            : _genderRepository.FindAll(x => x.Name.Contains(request.SearchTerm));

            //: _genderRepository.FindAll(x => x.Name.Contains(request.SearchTerm) || x.Description.Contains(request.SearchTerm));

            gendersQuery = request.SortOrder == SortOrder.Descending
            ? gendersQuery.OrderByDescending(GetSortProperty(request))
            : gendersQuery.OrderBy(GetSortProperty(request));

            var genders = await PagedResult<Domain.Entities.Gender>.CreateAsync(gendersQuery,
                request.PageIndex,
                request.PageSize);

            var result = _mapper.Map<PagedResult<Response.GenderResponse>>(genders);
            return Result.Success(result);
        }
    }

    // Func Sort for Get All Gender
    private static Expression<Func<Domain.Entities.Gender, object>> GetSortProperty(Query.GetAllGender request)
         => request.SortColumn?.ToLower() switch
         {
             "name" => gender => gender.Name,
             //"price" => gender => gender.Price, // Example fileds
             //"description" => gender => gender.Description, // Example fileds
             _ => gender => gender.Id // Default, If Sort Colunm == null
             //_ => gender => gender.CreatedDate // Default Sort Descending on CreatedDate column
         };
}
