using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;

namespace BadmintonSystem.Contract.Services.V1.ChatRoom;

public static class Query
{
    public record GetChatRoomsWithFilterAndSortQuery(
        Abstractions.Shared.Request.PagedFilterAndSortQueryRequest Data,
        Request.FilterChatRoomRequest FilterData)
        : IQuery<PagedResult<Response.GetChatRoomByIdResponse>>;
}
