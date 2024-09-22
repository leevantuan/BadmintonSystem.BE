using System.Linq.Expressions;
using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Enumerations;
using BadmintonSystem.Contract.Services.V2.Club;
using BadmintonSystem.Domain.Abstractions.Dappers;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V2.Club.Queries;
public sealed class GetAllClubQueryHandler : IQueryHandler<Query.GetAllClub, PagedResult<Response.ClubResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    private readonly IRepositoryBase<Domain.Entities.Club, Guid> _clubRepository;

    public GetAllClubQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ApplicationDbContext context, IRepositoryBase<Domain.Entities.Club, Guid> clubRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _context = context;
        _clubRepository = clubRepository;
    }

    public async Task<Result<PagedResult<Response.ClubResponse>>> Handle(Query.GetAllClub request, CancellationToken cancellationToken)
    {
        if (request.SortColumnAndOrder.Any())
        {
            var PageIndex = request.PageIndex <= 0 ? PagedResult<Domain.Entities.Club>.DefaultPageIndex : request.PageIndex;
            var PageSize = request.PageSize <= 0 ? PagedResult<Domain.Entities.Club>.DefaultPageSize
                : request.PageSize > PagedResult<Domain.Entities.Club>.DefaultPageSize
                ? PagedResult<Domain.Entities.Club>.UpperPageSize
                : request.PageSize;

            var clubsQuery = string.IsNullOrWhiteSpace(request.SearchTerm)
                ? @$"SELECT * FROM {nameof(Domain.Entities.Club)} ORDER BY "
                : $@"SELECT * FROM {nameof(Domain.Entities.Club)}
                              WHERE {nameof(Domain.Entities.Club.Name)} LIKE '%{request.SearchTerm}%' ORDER BY ";

            foreach (var item in request.SortColumnAndOrder)
                clubsQuery += item.Value == SortOrder.Descending
                    ? $"{item.Key} DESC, "
                    : $"{item.Key} ASC, ";

            clubsQuery = clubsQuery.Remove(clubsQuery.Length - 2);

            clubsQuery += $" OFFSET {(PageIndex - 1) * PageSize} ROWS FETCH NEXT {PageSize} ROWS ONLY";

            var clubs = await _context.Clubs.FromSqlRaw(clubsQuery)
                .ToListAsync(cancellationToken: cancellationToken);

            var totalCount = await _context.Clubs.CountAsync(cancellationToken);

            var clubPagedResult = PagedResult<Domain.Entities.Club>.Create(clubs,
                PageIndex,
                PageSize,
                totalCount);

            var result = _mapper.Map<PagedResult<Response.ClubResponse>>(clubPagedResult);

            return Result.Success(result);
        }
        else // =>> Entity Framework
        {
            var clubsQuery = string.IsNullOrWhiteSpace(request.SearchTerm)
            ? _clubRepository.FindAll()
            : _clubRepository.FindAll(x => x.Name.Contains(request.SearchTerm));


            clubsQuery = request.SortOrder == SortOrder.Descending
            ? clubsQuery.OrderByDescending(GetSortProperty(request))
            : clubsQuery.OrderBy(GetSortProperty(request));

            var genders = await PagedResult<Domain.Entities.Club>.CreateAsync(clubsQuery,
                request.PageIndex,
                request.PageSize);

            var result = _mapper.Map<PagedResult<Response.ClubResponse>>(genders);
            return Result.Success(result);
        }
    }

    private static Expression<Func<Domain.Entities.Club, object>> GetSortProperty(Query.GetAllClub request)
         => request.SortColumn?.ToLower() switch
         {
             "name" => club => club.Name,
             "code" => club => club.Code,
             "hotline" => club => club.HotLine,
             //"price" => club => club.Price, // Example fileds
             //"description" => club => club.Description, // Example fileds
             // _ => club => club.Id // Default, If Sort Colunm == null
             _ => club => club.DateCreated // Default Sort Descending on CreatedDate column
         };
}
