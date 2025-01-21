using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.ChatRoom;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Commands.ChatRoom;

public sealed class
    CreateChatRoomCommandHandler(
        ApplicationDbContext context,
        IMapper mapper,
        IRepositoryBase<Domain.Entities.ChatRoom, Guid> chatRoomRepository)
    : ICommandHandler<Command.CreateChatRoomCommand>
{
    public async Task<Result> Handle
    (
        Command.CreateChatRoomCommand request,
        CancellationToken cancellationToken)
    {
        // check exist
        Domain.Entities.ChatRoom? chatRoomExist =
            await context.ChatRoom.FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

        if (chatRoomExist != null)
        {
            throw new ChatRoomException.ChatRoomAlreadyExistException(request.UserId);
        }

        var chatRoom = new Domain.Entities.ChatRoom
        {
            UserId = request.UserId
        };

        chatRoomRepository.Add(chatRoom);

        await context.SaveChangesAsync(cancellationToken);

        Response.ChatRoomResponse? chatRoomResponse =
            mapper.Map<Response.ChatRoomResponse>(chatRoom);

        return Result.Success(chatRoomResponse);
    }
}
