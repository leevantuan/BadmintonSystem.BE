using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.YardType;

public static class Command
{
    public record CreateYardTypeCommand(Guid UserId, Request.CreateYardTypeRequest Data)
        : ICommand<Response.YardTypeResponse>;

    public record UpdateYardTypeCommand(Request.UpdateYardTypeRequest Data)
        : ICommand<Response.YardTypeResponse>;

    public record DeleteYardTypesCommand(List<string> Ids)
        : ICommand;
}
