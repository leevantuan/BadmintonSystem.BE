using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Enumerations;

namespace BadmintonSystem.Contract.Services.V2.AdditionalService;
public static class Query
{
    public record GetAdditionalServiceByIdQuery(Guid Id) : IQuery<Response.AdditionalServiceResponse>;

    public record GetAllAdditionalService(string? SearchTerm, string? SortColumn, SortOrder? SortOrder, IDictionary<string, SortOrder>? SortColumnAndOrder, int PageIndex, int PageSize) : IQuery<PagedResult<Response.AdditionalServiceResponse>>;

    public record GetAdditionalServiceByCategoryIdQuery(Guid Id) : IQuery<List<Response.AdditionalServiceResponse>>;
}

