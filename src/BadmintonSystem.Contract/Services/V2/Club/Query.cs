using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Enumerations;

namespace BadmintonSystem.Contract.Services.V2.Club;
public static class Query
{
    public record GetClubByIdQuery(Guid Id) : IQuery<Response.ClubResponse>;
    public record GetAllClub(string? SearchTerm, string? SortColumn, SortOrder? SortOrder, IDictionary<string, SortOrder>? SortColumnAndOrder, int PageIndex, int PageSize) : IQuery<PagedResult<Response.ClubResponse>>;
}

