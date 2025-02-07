using System.Text;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.ChatMessage;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using BadmintonSystem.Persistence.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Commands.ChatMessage;

public sealed class ReadAllByUserIdCommandHandler(
    ApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor)
    : ICommandHandler<Command.ReadAllByUserIdCommand>
{
    public async Task<Result> Handle(Command.ReadAllByUserIdCommand request, CancellationToken cancellationToken)
    {
        _ = await context.ChatRoom.FirstOrDefaultAsync(x => x.Id == request.ChatRoomId, cancellationToken)
            ?? throw new ChatRoomException.ChatRoomNotFoundException(request.ChatRoomId);

        List<string>? roles = httpContextAccessor.HttpContext?.GetCurrentRoles();
        bool isAdmin = roles?.Contains("Admin") ?? false;

        var readAllMessage = new StringBuilder();
        readAllMessage.Append(@$"UPDATE ""{nameof(Domain.Entities.ChatMessage)}""
                SET ""{nameof(Domain.Entities.ChatMessage.IsRead)}"" = true
                WHERE ""{nameof(Domain.Entities.ChatMessage.ChatRoomId)}"" = '{request.ChatRoomId}'
                AND ""{nameof(Domain.Entities.ChatMessage.IsAdmin)}"" = {!isAdmin}");

        await context.Database.ExecuteSqlRawAsync(readAllMessage.ToString(), cancellationToken);

        return Result.Success();
    }
}
