using System.Runtime.CompilerServices;
using System.Text;
using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Enumerations;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.User;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.User;

public sealed class GetChatMessagesByUserIdWithFilterAndSortQueryHandler(
    ApplicationDbContext context,
    IRepositoryBase<ChatMessage, Guid> chatMessageRepository,
    IMapper mapper)
    : IQueryHandler<Query.GetChatMessagesByUserIdWithFilterAndSortQuery,
        PagedResult<Response.GetChatMessageByUserIdResponse>>
{
    public async Task<Result<PagedResult<Response.GetChatMessageByUserIdResponse>>> Handle
        (Query.GetChatMessagesByUserIdWithFilterAndSortQuery request, CancellationToken cancellationToken)
    {
        // Get chat room
        Domain.Entities.ChatRoom chatRoom = context.ChatRoom.FirstOrDefault(x => x.UserId == request.UserId)
                                            ?? throw new ChatRoomException.ChatRoomNotFoundException(
                                                request.UserId,
                                                true);

        int pageIndex = request.Data.PageIndex <= 0
            ? PagedResult<ChatMessage>.DefaultPageIndex
            : request.Data.PageIndex;

        int pageSize = request.Data.PageSize <= 0
            ? PagedResult<ChatMessage>.DefaultPageSize
            : request.Data.PageSize > PagedResult<ChatMessage>.UpperPageSize
                ? PagedResult<ChatMessage>.UpperPageSize
                : request.Data.PageSize;

        string chatRoomColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.ChatRoom,
                Contract.Services.V1.ChatRoom.Response.ChatRoomResponse>();

        string chatMessageColumns = StringExtension
            .TransformPropertiesToSqlAliases<ChatMessage,
                Contract.Services.V1.ChatMessage.Response.ChatMessageResponse>();

        var baseQueryBuilder = new StringBuilder();
        baseQueryBuilder.Append(
            $@"FROM ""{nameof(ChatMessage)}"" AS chatMessage
                WHERE chatMessage.""{nameof(ChatMessage.IsDeleted)}"" = false
                AND chatMessage.""{nameof(ChatMessage.ChatRoomId)}"" = '{chatRoom.Id}' ");

        if (!string.IsNullOrWhiteSpace(request.Data.SearchTerm))
        {
            baseQueryBuilder.Append(
                $@"AND chatMessage.""{nameof(ChatMessage.Content)}"" ILIKE '%{request.Data.SearchTerm}%' ");
        }

        if (request.Data.FilterColumnAndMultipleValue.Any())
        {
            foreach (KeyValuePair<string, List<string>> item in request.Data.FilterColumnAndMultipleValue)
            {
                string key = ChatMessageExtension.GetSortChatMessageProperty(item.Key);
                baseQueryBuilder.Append($@"AND chatMessage.""{key}""::TEXT ILIKE ANY (ARRAY[");

                foreach (string value in item.Value)
                {
                    baseQueryBuilder.Append($@"'%{value}%', ");
                }

                // Remove trailing comma
                baseQueryBuilder.Length -= 2;

                baseQueryBuilder.Append("]) ");
            }
        }

        if (request.Data.SortColumnAndOrder.Any())
        {
            baseQueryBuilder.Append("\nORDER BY");
            foreach (KeyValuePair<string, SortOrder> item in request.Data.SortColumnAndOrder)
            {
                string key = ChatMessageExtension.GetSortChatMessageProperty(item.Key);
                baseQueryBuilder.Append(item.Value == SortOrder.Descending
                    ? $@" chatMessage.""{key}"" DESC, "
                    : $@" chatMessage.""{key}"" ASC, ");
            }

            // Remove trailing comma and space
            baseQueryBuilder.Length -= 2;
        }

        // Handle chat message query
        var chatMessagesQueryBuilder = new StringBuilder();
        chatMessagesQueryBuilder.Append($@"SELECT {chatMessageColumns}");
        chatMessagesQueryBuilder.Append(" \n");
        chatMessagesQueryBuilder.Append(baseQueryBuilder);

        // Calculate total count record
        var countQueryBuilder = new StringBuilder();
        countQueryBuilder.Append(
            $@"SELECT COUNT(DISTINCT chatMessage.""{nameof(ChatMessage.Id)}"") AS ""{nameof(SqlResponse.TotalCountSqlResponse.TotalCount)}""");
        countQueryBuilder.Append(" \n");

        // Exclude ORDER BY from baseQueryBuilder
        string baseQueryWithoutOrderBy = baseQueryBuilder.ToString();
        int orderByIndex = baseQueryWithoutOrderBy.LastIndexOf("ORDER BY", StringComparison.OrdinalIgnoreCase);
        if (orderByIndex > -1)
        {
            baseQueryWithoutOrderBy = baseQueryWithoutOrderBy.Substring(0, orderByIndex);
        }

        countQueryBuilder.Append(baseQueryWithoutOrderBy);

        SqlResponse.TotalCountSqlResponse totalCountQueryResult = await chatMessageRepository
            .ExecuteSqlQuery<SqlResponse.TotalCountSqlResponse>(
                FormattableStringFactory.Create(countQueryBuilder.ToString()))
            .SingleAsync(cancellationToken);

        chatMessagesQueryBuilder.Append(
            $"\nOFFSET {(pageIndex - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY");

        List<Response.GetChatMessageByUserIdSqlResponse> queryResult = await chatMessageRepository
            .ExecuteSqlQuery<Response.GetChatMessageByUserIdSqlResponse>(
                FormattableStringFactory.Create(chatMessagesQueryBuilder.ToString()))
            .ToListAsync(cancellationToken);

        var chatMessages = queryResult.GroupBy(r => r.ChatMessage_Id)
            .Select(g => new Response.GetChatMessageByUserIdResponse
            {
                Id = g.Key,
                Content = g.First().ChatMessage_Content,
                ImageUrl = g.First().ChatMessage_ImageUrl,
                IsAdmin = g.First().ChatMessage_IsAdmin,
                IsRead = g.First().ChatMessage_IsRead,
                ReadDate = g.First().ChatMessage_ReadDate,
                ChatRoomId = g.First().ChatMessage_ChatRoomId ?? chatRoom.Id,
                CreatedDate = g.First().ChatMessage_CreatedDate,
            })
            .ToList();

        var chatMessagePagedResult =
            PagedResult<Response.GetChatMessageByUserIdResponse>.Create(
                chatMessages,
                pageIndex,
                pageSize,
                totalCountQueryResult.TotalCount);

        return Result.Success(chatMessagePagedResult);
    }
}
