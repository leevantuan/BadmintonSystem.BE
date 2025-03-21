using AutoMapper;
using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.ChatMessage;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BadmintonSystem.Application.UseCases.V1.Commands.ChatMessage;

public sealed class
    CreateChatMessageCommandHandler(
        ApplicationDbContext context,
        UserManager<AppUser> userManager,
        IMapper mapper,
        IChatHub chatHub,
        IHttpContextAccessor httpContext,
        IRepositoryBase<Domain.Entities.ChatMessage, Guid> chatMessageRepository)
    : ICommandHandler<Command.CreateChatMessageCommand, Response.ChatMessageResponse>
{
    public async Task<Result<Response.ChatMessageResponse>> Handle
    (
        Command.CreateChatMessageCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Data.UserId is null)
        {
            throw new IdentityException.AppUserException("User Id not null");
        }

        // Get chat room
        Domain.Entities.ChatRoom chatRoom = context.ChatRoom.FirstOrDefault(x => x.UserId == request.Data.UserId)
                                            ?? throw new ChatRoomException.ChatRoomNotFoundException(
                                                request.Data.UserId ?? Guid.Empty,
                                                true);

        var chatMessage = new Domain.Entities.ChatMessage
        {
            ImageUrl = request.Data.ImageUrl,
            Content = request.Data.Content,
            IsAdmin = request.Data.IsAdmin,
            IsRead = false,
            ReadDate = DateTime.Now,
            ChatRoomId = chatRoom.Id
        };

        chatMessageRepository.Add(chatMessage);

        await context.SaveChangesAsync(cancellationToken);

        Response.ChatMessageResponse? chatMessageResponse =
            mapper.Map<Response.ChatMessageResponse>(chatMessage);

        AppUser? admin = await userManager.FindByEmailAsync("admin@gmail.com");

        Guid? receiverMessage = chatMessage.IsAdmin ? request.Data.UserId : admin.Id;

        await chatHub.SendMessageToUserAsync(receiverMessage.ToString(), chatMessageResponse);

        return Result.Success(chatMessageResponse);
    }
}
