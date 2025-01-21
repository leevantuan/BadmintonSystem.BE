using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.ChatRoom;
using BadmintonSystem.Persistence.Helpers;
using BadmintonSystem.Presentation.Abstractions;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Request = BadmintonSystem.Contract.Abstractions.Shared.Request;

namespace BadmintonSystem.Presentation.APIs.V1;

public class ChatRoomApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/chat-rooms";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group1 = app.NewVersionedApi("chat-rooms")
            .MapGroup(BaseUrl)
            .HasApiVersion(1)
            .RequireAuthorization();

        group1.MapPost("filter-and-sort", GetChatRoomsWithFilterAndSortV1);
        group1.MapPost(string.Empty, CreateChatRoomV1);
    }

    private static async Task<IResult> GetChatRoomsWithFilterAndSortV1
    (
        ISender sender,
        [FromBody] Contract.Services.V1.ChatRoom.Request.FilterChatRoomRequest filterRequest,
        [AsParameters] Request.PagedFilterAndSortRequest request)
    {
        var pagedQueryRequest = new Request.PagedFilterAndSortQueryRequest(request);
        Result<PagedResult<Response.GetChatRoomByIdResponse>> result =
            await sender.Send(
                new Query.GetChatRoomsWithFilterAndSortQuery(pagedQueryRequest, filterRequest));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> CreateChatRoomV1
    (
        ISender sender,
        IHttpContextAccessor httpContextAccessor)
    {
        Guid? userId = httpContextAccessor.HttpContext?.GetCurrentUserId();

        Result result = await sender.Send(new Command.CreateChatRoomCommand(userId ?? Guid.Empty));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }
}
