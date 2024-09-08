using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Enumerations;

namespace BadmintonSystem.Contract.Services.V2.Gender;
public static class Query
{
    public record GetGenderByIdQuery(Guid Id) : IQuery<Response.GenderResponse>;
    public record GetAllGender(string? SearchTerm, string? SortColumn, SortOrder? SortOrder, IDictionary<string, SortOrder>? SortColumnAndOrder, int PageIndex, int PageSize) : IQuery<PagedResult<Response.GenderResponse>>;
}
