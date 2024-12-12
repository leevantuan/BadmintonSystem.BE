using System.Text;
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

public sealed class GetClubsWithFilterAndSortValueQueryHandler(
    IMapper mapper,
    ApplicationDbContext context,
    IRepositoryBase<Domain.Entities.Club, Guid> clubRepository)
    : IQueryHandler<Query.GetClubsWithFilterAndSortValueQuery, PagedResult<Response.ClubDetailResponse>>
{
    public async Task<Result<PagedResult<Response.ClubDetailResponse>>> Handle
        (Query.GetClubsWithFilterAndSortValueQuery request, CancellationToken cancellationToken)
    {
        // Page Index and Page Size
        int PageIndex = request.Data.PageIndex <= 0
            ? PagedResult<Domain.Entities.Club>.DefaultPageIndex
            : request.Data.PageIndex;
        int PageSize = request.Data.PageSize <= 0
            ? PagedResult<Domain.Entities.Club>.DefaultPageSize
            : request.Data.PageSize > PagedResult<Domain.Entities.Club>.UpperPageSize
                ? PagedResult<Domain.Entities.Club>.UpperPageSize
                : request.Data.PageSize;

        // Handle Query SQL
        var clubsQuery = new StringBuilder();

        clubsQuery.Append($@"SELECT * FROM ""{nameof(Domain.Entities.Club)}""
                             WHERE ""{nameof(Domain.Entities.Club.Name)}"" ILIKE '%{request.Data.SearchTerm}%'");

        if (request.Data.FilterColumnAndMultipleValue.Any())
        {
            foreach (KeyValuePair<string, List<string>> item in request.Data.FilterColumnAndMultipleValue)
            {
                string key = ClubExtension.GetSortClubProperty(item.Key);
                clubsQuery.Append($@"AND ""{nameof(Domain.Entities.Club)}"".""{key}""::TEXT ILIKE ANY (ARRAY[");

                foreach (string value in item.Value)
                {
                    clubsQuery.Append($@"'%{value}%', ");
                }

                clubsQuery.Length -= 2;

                clubsQuery.Append("]) ");
            }
        }

        if (request.Data.SortColumnAndOrder.Any())
        {
            clubsQuery.Append("ORDER BY ");
            foreach (KeyValuePair<string, SortOrder> item in request.Data.SortColumnAndOrder)
            {
                string key = ReviewExtension.GetSortReviewProperty(item.Key);
                clubsQuery.Append(item.Value == SortOrder.Descending
                    ? $@" ""{nameof(Domain.Entities.Club)}"".""{key}"" DESC, "
                    : $@" ""{nameof(Domain.Entities.Club)}"".""{key}"" ASC, ");
            }

            clubsQuery.Length -= 2;
        }

        clubsQuery.Append($"\nOFFSET {(PageIndex - 1) * PageSize} ROWS FETCH NEXT {PageSize} ROWS ONLY");

        List<Domain.Entities.Club> clubs =
            await context.Club.FromSqlRaw(clubsQuery.ToString()).ToListAsync(cancellationToken);

        int totalCount = clubs.Count();

        var clubPagedResult = PagedResult<Domain.Entities.Club>.Create(clubs, PageIndex, PageSize, totalCount);

        PagedResult<Response.ClubDetailResponse>? result =
            mapper.Map<PagedResult<Response.ClubDetailResponse>>(clubPagedResult);

        return Result.Success(result);
    }
}
