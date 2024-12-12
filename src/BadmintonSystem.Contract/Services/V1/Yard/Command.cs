using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.Yard;

public static class Command
{
    public record CreateYardCommand(Guid UserId, Request.CreateYardRequest Data)
        : ICommand<Response.YardResponse>;

    public record UpdateYardCommand(Request.UpdateYardRequest Data)
        : ICommand<Response.YardResponse>;

    public record DeleteYardsCommand(List<string> Ids)
        : ICommand;
}
