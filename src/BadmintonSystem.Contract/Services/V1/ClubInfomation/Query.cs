using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;

namespace BadmintonSystem.Contract.Services.V1.ClubInformation;

public static class Query
{
    public record GetClubInformationsQuery(Abstractions.Shared.Request.PagedQueryRequest Data)
        : IQuery<PagedResult<Response.ClubInformationResponse>>;

    public record GetClubInformationByIdQuery(Guid Id)
        : IQuery<Response.ClubInformationResponse>;
}
