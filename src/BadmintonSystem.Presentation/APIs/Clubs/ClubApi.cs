using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Presentation.Abstractions;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace BadmintonSystem.Presentation.APIs.Clubs;
public class ClubApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/clubs";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.NewVersionedApi("Club")
            .MapGroup(BaseUrl).HasApiVersion(2);

        group.MapGet(string.Empty, GetAllClub);
        group.MapGet("{ClubId}", GetByIdClub);
        group.MapPost(string.Empty, CreateClub);
        group.MapPut("{ClubId}", UpdateClub);
        group.MapDelete("{ClubId}", DeleteClub);
    }

    public static async Task<IResult> CreateClub(ISender sender, [FromBody] Contract.Services.V2.Club.Command.CreateClubCommand CreateClub)
    {
        var result = await sender.Send(CreateClub);

        return Results.Ok(result);
    }

    public static async Task<IResult> DeleteClub(ISender sender, Guid ClubId)
    {
        var result = await sender.Send(new Contract.Services.V2.Club.Command.DeleteClubCommand(ClubId));
        return Results.Ok(result);
    }

    public static async Task<IResult> GetAllClub(ISender sender, string? searchTerm = null,
        string? sortColumn = null,
        string? sortOrder = null,
        string? sortColumnAndOrder = null,
        int pageIndex = 1,
        int pageSize = 10)
    {
        var result = await sender.Send(new Contract.Services.V2.Club.Query.GetAllClub(searchTerm, sortColumn,
            SortOrderExtension.ConvertStringToSortOrder(sortOrder),
            SortOrderExtension.ConvertStringToSortOrderV2(sortColumnAndOrder),
            pageIndex,
            pageSize));
        return Results.Ok(result);
    }

    public static async Task<IResult> GetByIdClub(ISender sender, Guid ClubId)
    {
        var result = await sender.Send(new Contract.Services.V2.Club.Query.GetClubByIdQuery(ClubId));
        return Results.Ok(result);
    }

    public static async Task<IResult> UpdateClub(ISender sender, Guid ClubId, [FromBody] Contract.Services.V2.Club.Command.UpdateClubCommand updateClub)
    {
        var updateClubCommand = new Contract.Services.V2.Club.Command.UpdateClubCommand(ClubId, updateClub.Data);
        return Results.Ok(await sender.Send(updateClubCommand));
    }
}

