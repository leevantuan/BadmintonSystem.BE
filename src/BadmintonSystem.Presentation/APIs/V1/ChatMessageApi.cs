using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.ChatMessage;
using BadmintonSystem.Persistence.Helpers;
using BadmintonSystem.Presentation.Abstractions;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Request = BadmintonSystem.Contract.Services.V1.ChatMessage.Request;

namespace BadmintonSystem.Presentation.APIs.V1;

public class ChatMessageApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/chat-messages";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group1 = app.NewVersionedApi("chat-messages")
            .MapGroup(BaseUrl)
            .HasApiVersion(1)
            .RequireAuthorization();

        // Chat
        group1.MapPost(string.Empty, CreateChatMessageV1).AllowAnonymous();

        group1.MapGet(string.Empty, ReadAllMessageV1).AllowAnonymous();

        // Chatbot
        group1.MapPost("chatbot", CreateChatMessageByChatbotV1).AllowAnonymous();
    }

    private static async Task<IResult> CreateChatMessageV1
    (
        ISender sender,
        [FromBody] Request.CreateChatMessageRequest request)
    {
        Result<Response.ChatMessageResponse> result =
            await sender.Send(new Command.CreateChatMessageCommand(request));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> ReadAllMessageV1
    (
        ISender sender,
        IHttpContextAccessor httpContextAccessor,
        [FromQuery] Guid chatRoomId)
    {
        Guid? userId = httpContextAccessor.HttpContext?.GetCurrentUserId();

        Result result = await sender.Send(new Command.ReadAllByUserIdCommand(chatRoomId, userId ?? Guid.Empty));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> CreateChatMessageByChatbotV1
    (
        ISender sender,
        [FromBody] Request.CreateChatMessageByChatbotRequest request)
    {
        Result<Response.ChatbotResponse> result =
            await sender.Send(new Command.CreateChatMessageByChatbotCommand(request));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }
}
