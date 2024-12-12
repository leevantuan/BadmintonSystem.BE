using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;

namespace BadmintonSystem.Contract.Services.V1.ClubAddress;

public static class Query
{
    public record GetClubAddressesQuery(Abstractions.Shared.Request.PagedQueryRequest Data)
        : IQuery<PagedResult<Response.ClubAddressResponse>>;

    public record GetClubAddressByIdQuery(Guid Id)
        : IQuery<Response.ClubAddressResponse>;
}
