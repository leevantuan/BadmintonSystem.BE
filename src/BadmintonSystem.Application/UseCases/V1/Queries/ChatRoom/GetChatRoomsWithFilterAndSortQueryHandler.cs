﻿using AutoMapper;
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

        var chatRoomByUserId = await context.ChatRoom.Where(x => x.IsDeleted == false && x.UserId == request.FilterData.UserId).ToListAsync(cancellationToken);

        if (chatRoomByUserId.Count == 0)
        {
            if (request.FilterData.UserId == null)
            {
                throw new ApplicationException("User Id is required");
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

        //var chatRoomPagedResult =
        //    PagedResult<Response.GetChatRoomByIdResponse>.Create(
        //        chatRooms,
        //        pageIndex,
        //        pageSize,
        //        totalCountQueryResult.TotalCount);

        //int pageIndex = request.Data.PageIndex <= 0
        //    ? PagedResult<Domain.Entities.ChatRoom>.DefaultPageIndex
        //    : request.Data.PageIndex;

        //int pageSize = request.Data.PageSize <= 0
        //    ? PagedResult<Domain.Entities.ChatRoom>.DefaultPageSize
        //    : request.Data.PageSize > PagedResult<Domain.Entities.ChatRoom>.UpperPageSize
        //        ? PagedResult<Domain.Entities.ChatRoom>.UpperPageSize
        //        : request.Data.PageSize;

        //string chatRoomColumns = StringExtension
        //    .TransformPropertiesToSqlAliases<Domain.Entities.ChatRoom,
        //        Response.ChatRoomResponse>();

        //string chatMessageColumns = StringExtension
        //    .TransformPropertiesToSqlAliases<ChatMessage,
        //        Contract.Services.V1.ChatMessage.Response.ChatMessageResponse>();

        //string appUserColumns = StringExtension
        //    .TransformPropertiesToSqlAliases<AppUser,
        //        Contract.Services.V1.User.Response.AppUserResponse>();

        //var chatMessageLatestQueryBuilder = new StringBuilder();

        //chatMessageLatestQueryBuilder.Append(
        //    $@"With ChatMessageLatest AS (
        //        SELECT DISTINCT ON (""{nameof(ChatMessage.ChatRoomId)}"") chatMessage.*
        //        FROM ""{nameof(ChatMessage)}"" AS chatMessage
        //        WHERE ""{nameof(ChatMessage.IsDeleted)}"" = false
        //        ORDER BY ""{nameof(ChatMessage.ChatRoomId)}"", ""{nameof(ChatMessage.CreatedDate)}"" DESC
        //    )");

        //// close parentheses
        //chatMessageLatestQueryBuilder.Append(" \n");

        //var baseQueryBuilder = new StringBuilder();

        //baseQueryBuilder.Append(
        //    $@"FROM ""{nameof(Domain.Entities.ChatRoom)}"" AS chatRoom
        //        LEFT JOIN ChatMessageLatest AS chatMessage
        //        ON chatMessage.""{nameof(ChatMessage.ChatRoomId)}"" = chatRoom.""{nameof(Domain.Entities.ChatRoom.Id)}""
        //        LEFT JOIN ""{nameof(context.AppUsers)}"" AS appUser
        //        ON appUser.""{nameof(AppUser.Id)}"" = chatRoom.""{nameof(Domain.Entities.ChatRoom.UserId)}""
        //        AND appUser.""{nameof(AppUser.IsDeleted)}"" = false 
        //        LEFT JOIN ""{nameof(context.AppUserRoles)}"" AS appUserRole
        //        ON appUserRole.""{nameof(AppUserRole.UserId)}"" = chatRoom.""{nameof(Domain.Entities.ChatRoom.UserId)}""
        //        LEFT JOIN ""{nameof(context.AppRoles)}"" AS appRole
        //        ON appRole.""{nameof(AppRole.Id)}"" = appUserRole.""{nameof(AppUserRole.RoleId)}""
        //        AND appRole.""{nameof(AppRole.IsDeleted)}"" = false 
        //        WHERE chatRoom.""{nameof(Domain.Entities.ChatRoom.IsDeleted)}"" = false
        //        AND appRole.""{nameof(AppRole.RoleCode)}"" = '{ConvertAppRoleTypeFromNumberToString(request.FilterData.AppRoleType)}' ");

        //if (!string.IsNullOrWhiteSpace(request.Data.SearchTerm))
        //{
        //    baseQueryBuilder.Append(
        //        $@"AND appUser.""{nameof(AppUser.FullName)}"" ILIKE '%{request.Data.SearchTerm}%' ");
        //}

        //if (request.Data.FilterColumnAndMultipleValue.Any())
        //{
        //    foreach (KeyValuePair<string, List<string>> item in request.Data.FilterColumnAndMultipleValue)
        //    {
        //        string key = AppUserExtension.GetSortAppUserProperty(item.Key);
        //        baseQueryBuilder.Append($@"AND appUser.""{key}""::TEXT ILIKE ANY (ARRAY[");

        //        foreach (string value in item.Value)
        //        {
        //            baseQueryBuilder.Append($@"'%{value}%', ");
        //        }

        //        // Remove trailing comma
        //        baseQueryBuilder.Length -= 2;

        //        baseQueryBuilder.Append("]) ");
        //    }
        //}

        //if (request.Data.SortColumnAndOrder.Any())
        //{
        //    baseQueryBuilder.Append("ORDER BY ");
        //    foreach (KeyValuePair<string, SortOrder> item in request.Data.SortColumnAndOrder)
        //    {
        //        string key = ChatMessageExtension.GetSortChatMessageProperty(item.Key);
        //        baseQueryBuilder.Append(item.Value == SortOrder.Descending
        //            ? $@" chatMessage.""{key}"" DESC, "
        //            : $@" chatMessage.""{key}"" ASC, ");
        //    }

        //    // Remove trailing comma and space
        //    baseQueryBuilder.Length -= 2;
        //}

        //// Handle chat message query
        //var chatRoomsQueryBuilder = new StringBuilder(chatMessageLatestQueryBuilder.ToString());
        //chatRoomsQueryBuilder.Append($@"SELECT {chatRoomColumns}, {chatMessageColumns}, {appUserColumns}");
        //chatRoomsQueryBuilder.Append(" \n");
        //chatRoomsQueryBuilder.Append(baseQueryBuilder);

        //// Calculate total count record
        //var countQueryBuilder = new StringBuilder(chatMessageLatestQueryBuilder.ToString());
        //countQueryBuilder.Append(
        //    $@"SELECT COUNT(DISTINCT chatRoom.""{nameof(Domain.Entities.ChatRoom.Id)}"") AS ""{nameof(SqlResponse.TotalCountSqlResponse.TotalCount)}""");
        //countQueryBuilder.Append(" \n");

        //// Exclude ORDER BY from baseQueryBuilder
        //string baseQueryWithoutOrderBy = baseQueryBuilder.ToString();
        //int orderByIndex = baseQueryWithoutOrderBy.LastIndexOf("ORDER BY", StringComparison.OrdinalIgnoreCase);
        //if (orderByIndex > -1)
        //{
        //    baseQueryWithoutOrderBy = baseQueryWithoutOrderBy.Substring(0, orderByIndex);
        //}

        //countQueryBuilder.Append(baseQueryWithoutOrderBy);

        //SqlResponse.TotalCountSqlResponse totalCountQueryResult = await chatRoomRepository
        //    .ExecuteSqlQuery<SqlResponse.TotalCountSqlResponse>(
        //        FormattableStringFactory.Create(countQueryBuilder.ToString()))
        //    .SingleAsync(cancellationToken);

        //chatRoomsQueryBuilder.Append($"\nOFFSET {(pageIndex - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY");

        //List<Response.GetChatRoomByIdSqlResponse> queryResult = await chatRoomRepository
        //    .ExecuteSqlQuery<Response.GetChatRoomByIdSqlResponse>(
        //        FormattableStringFactory.Create(chatRoomsQueryBuilder.ToString()))
        //    .ToListAsync(cancellationToken);

        //var chatRooms = queryResult.GroupBy(r => r.ChatRoom_Id)
        //    .Select(g => new Response.GetChatRoomByIdResponse
        //    {
        //        Id = g.Key,
        //        UserId = g.First().ChatRoom_UserId,
        //        CreatedDate = g.First().ChatRoom_CreatedDate,

        //        ChatMessage = g.Select(s => new Contract.Services.V1.ChatMessage.Response.ChatMessageResponse
        //        {
        //            Id = s.ChatMessage_Id ?? Guid.Empty,
        //            ImageUrl = s.ChatMessage_ImageUrl,
        //            Content = s.ChatMessage_Content,
        //            IsAdmin = s.ChatMessage_IsAdmin ?? false,
        //            IsRead = s.ChatMessage_IsRead ?? false,
        //            ReadDate = s.ChatMessage_ReadDate,
        //            ChatRoomId = s.ChatMessage_ChatRoomId ?? Guid.Empty,
        //            CreatedDate = s.ChatMessage_CreatedDate ?? DateTime.Now
        //        })
        //            .FirstOrDefault(),

        //        User = g.Select(s => new Contract.Services.V1.User.Response.AppUserResponse
        //        {
        //            //Id = s.AppUser_Id ?? Guid.Empty,
        //            //UserName = s.AppUser_UserName ?? string.Empty,
        //            //Email = s.AppUser_Email ?? string.Empty,
        //            //FullName = s.AppUser_FullName ?? string.Empty,
        //            //DateOfBirth = s.AppUser_DateOfBirth,
        //            //AvatarUrl = s.AppUser_AvatarUrl ?? string.Empty
        //        })
        //            .FirstOrDefault()
        //    })
        //    .ToList();

        //var chatRoomPagedResult =
        //    PagedResult<Response.GetChatRoomByIdResponse>.Create(
        //        chatRooms,
        //        pageIndex,
        //        pageSize,
        //        totalCountQueryResult.TotalCount);

        //return Result.Success(chatRoomPagedResult);
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
