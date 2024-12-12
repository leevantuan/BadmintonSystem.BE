using System.Linq.Expressions;
using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Enumerations;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Club;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Club;

public sealed class
    GetClubsQueryHandler(
        ApplicationDbContext context,
        IMapper mapper,
        IRepositoryBase<Domain.Entities.Club, Guid> clubRepository)
    : IQueryHandler<Query.GetClubsQuery, PagedResult<Response.ClubResponse>>
{
    public async Task<Result<PagedResult<Response.ClubResponse>>> Handle
    (Query.GetClubsQuery request,
        CancellationToken cancellationToken)
    {
        if (request.Data.SortColumnAndOrder.Any())
        {
            int PageIndex = request.Data.PageIndex <= 0
                ? PagedResult<Domain.Entities.Club>.DefaultPageIndex
                : request.Data.PageIndex;
            int PageSize = request.Data.PageSize <= 0
                ? PagedResult<Domain.Entities.Club>.DefaultPageSize
                : request.Data.PageSize > PagedResult<Domain.Entities.Club>.UpperPageSize
                    ? PagedResult<Domain.Entities.Club>.UpperPageSize
                    : request.Data.PageSize;

            string clubsQuery = string.IsNullOrWhiteSpace(request.Data.SearchTerm)
                ? $@"SELECT * FROM ""{nameof(Domain.Entities.Club)}"" ORDER BY "
                : $@"SELECT * FROM ""{nameof(Domain.Entities.Club)}""
                              WHERE ""{nameof(Domain.Entities.Club.Name)}"" LIKE '%{request.Data.SearchTerm}%' 
                              ORDER BY ";

            foreach (KeyValuePair<string, SortOrder> item in request.Data.SortColumnAndOrder)
            {
                string key = ClubExtension.GetSortClubProperty(item.Key);

                clubsQuery += item.Value == SortOrder.Descending
                    ? $"\"{key}\" DESC, "
                    : $"\"{key}\" ASC, ";
            }

            clubsQuery = clubsQuery.Remove(clubsQuery.Length - 2);

            clubsQuery += $" OFFSET {(PageIndex - 1) * PageSize} ROWS FETCH NEXT {PageSize} ROWS ONLY";

            List<Domain.Entities.Club> clubs =
                await context.Club.FromSqlRaw(clubsQuery).ToListAsync(cancellationToken);

            int totalCount = await context.Club.CountAsync(cancellationToken);

            var clubPagedResult =
                PagedResult<Domain.Entities.Club>.Create(clubs, PageIndex, PageSize, totalCount);

            PagedResult<Response.ClubResponse>? result =
                mapper.Map<PagedResult<Response.ClubResponse>>(clubPagedResult);

            return Result.Success(result);
        }
        else
        {
            IQueryable<Domain.Entities.Club> clubsQuery = string.IsNullOrWhiteSpace(request.Data.SearchTerm)
                ? clubRepository.FindAll()
                : clubRepository.FindAll(x => x.Name.Contains(request.Data.SearchTerm));

            clubsQuery = request.Data.SortOrder == SortOrder.Descending
                ? clubsQuery.OrderByDescending(GetSortColumnProperty(request))
                : clubsQuery.OrderBy(GetSortColumnProperty(request));

            var clubs = await PagedResult<Domain.Entities.Club>.CreateAsync(clubsQuery, request.Data.PageIndex,
                request.Data.PageSize);

            PagedResult<Response.ClubResponse>? result = mapper.Map<PagedResult<Response.ClubResponse>>(clubs);

            return Result.Success(result);
        }
    }

    private static Expression<Func<Domain.Entities.Club, object>> GetSortColumnProperty
    (
        Query.GetClubsQuery request)
    {
        return request.Data.SortColumn?.Trim().ToLower() switch
        {
            "name" => club => club.Name,
            _ => club => club.Id
        };
    }
}
