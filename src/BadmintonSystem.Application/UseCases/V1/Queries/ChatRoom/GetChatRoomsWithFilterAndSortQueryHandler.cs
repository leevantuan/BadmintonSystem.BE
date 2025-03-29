using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.ChatRoom;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.ChatRoom;

public sealed class GetChatRoomsWithFilterAndSortQueryHandler(
    ApplicationDbContext context,
    IRepositoryBase<Domain.Entities.ChatRoom, Guid> chatRoomRepository,
    IMapper mapper)
    : IQueryHandler<Query.GetChatRoomsWithFilterAndSortQuery, PagedResult<Response.GetChatRoomByIdResponse>>
{
    public async Task<Result<PagedResult<Response.GetChatRoomByIdResponse>>> Handle
    (
        Query.GetChatRoomsWithFilterAndSortQuery request, CancellationToken cancellationToken)
    {
        // Pagination initialization
        int pageIndex = request.Data.PageIndex <= 0
            ? PagedResult<Domain.Entities.ChatRoom>.DefaultPageIndex
            : request.Data.PageIndex;

        int pageSize = request.Data.PageSize <= 0
            ? PagedResult<Domain.Entities.ChatRoom>.DefaultPageSize
            : request.Data.PageSize > PagedResult<Domain.Entities.ChatRoom>.UpperPageSize
                ? PagedResult<Domain.Entities.ChatRoom>.UpperPageSize
                : request.Data.PageSize;

        // Check isAdmin role
        var roleName = ConvertAppRoleTypeFromNumberToString(request.FilterData.AppRoleType);

        if (roleName == AppRoleEnum.ADMIN.ToString())
        {
            var queryChatRoom = await (from chatRoom in context.ChatRoom
                                       join chatMess in context.ChatMessage
                                       on chatRoom.Id equals chatMess.ChatRoomId into chatMessageGroup
                                       from chatMessage in chatMessageGroup.DefaultIfEmpty()
                                       where chatRoom.IsDeleted == false && (chatMessage == null || chatMessage.IsDeleted == false)
                                       select new
                                       {
                                           chatRoom,
                                           chatMessage
                                       }).ToListAsync(cancellationToken: cancellationToken);

            var chatRooms = queryChatRoom.GroupBy(x => x.chatRoom.Id)
                .Select(x => new Response.GetChatRoomByIdResponse
                {
                    Id = x.Key,
                    UserId = x.First().chatRoom.UserId,
                    UserName = x.First().chatRoom.UserName,
                    Email = x.First().chatRoom.Email,
                    Avatar = x.First().chatRoom.Avatar,
                    ChatMessage = x.Select(s => new Contract.Services.V1.ChatMessage.Response.ChatMessageResponse
                    {
                        Id = s.chatMessage.Id,
                        ImageUrl = s.chatMessage.ImageUrl,
                        Content = s.chatMessage.Content,
                        IsAdmin = s.chatMessage.IsAdmin,
                        IsRead = s.chatMessage.IsRead,
                        ReadDate = s.chatMessage.ReadDate,
                        ChatRoomId = s.chatMessage.ChatRoomId,
                        CreatedDate = s.chatMessage.CreatedDate
                    }).OrderByDescending(s => s.CreatedDate) // Sắp xếp theo CreatedDate (mới nhất lên đầu)
                        .FirstOrDefault()
                }).ToList();

            var chatRoomPagedResult =
                PagedResult<Response.GetChatRoomByIdResponse>.Create(
                    chatRooms.Select(x => mapper.Map<Response.GetChatRoomByIdResponse>(x)).ToList(),
                    request.Data.PageIndex,
                    request.Data.PageSize,
                    chatRooms.Count);

            return Result.Success(chatRoomPagedResult);
        }

        var chatRoomByUserId = await context.ChatRoom.Where(x => x.IsDeleted == false && x.Email == request.FilterData.Email).ToListAsync(cancellationToken);

        if (chatRoomByUserId.Count == 0)
        {
            if (request.FilterData.Email == null)
            {
                throw new ApplicationException("User Email is required");
            }

            var chatRoomEntities = new Domain.Entities.ChatRoom
            {
                UserId = request.FilterData.UserId ?? Guid.Empty,
                UserName = request.FilterData.UserName ?? string.Empty,
                Email = request.FilterData.Email ?? string.Empty,
                Avatar = request.FilterData.Avatar ?? string.Empty,
            };

            context.ChatRoom.Add(chatRoomEntities);
            await context.SaveChangesAsync(cancellationToken);

            chatRoomByUserId.Add(chatRoomEntities);
        }

        var chatRoomByUserIdPagedResult =
            PagedResult<Response.GetChatRoomByIdResponse>.Create(
                chatRoomByUserId.Select(x => mapper.Map<Response.GetChatRoomByIdResponse>(x)).ToList(),
                request.Data.PageIndex,
                request.Data.PageSize,
                chatRoomByUserId.Count);

        return Result.Success(chatRoomByUserIdPagedResult);
    }

    private static string ConvertAppRoleTypeFromNumberToString(int appRoleTypeRequest)
    {
        int appRoleTypeHandled = Enum.IsDefined(typeof(AppRoleEnum), appRoleTypeRequest)
            ? appRoleTypeRequest
            : (int)AppRoleEnum.CUSTOMER;

        string filterAppRoleTypeString = ((AppRoleEnum)appRoleTypeHandled).ToString();

        return filterAppRoleTypeString;
    }
}
