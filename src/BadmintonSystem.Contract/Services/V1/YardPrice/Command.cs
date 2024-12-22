using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.YardPrice;

public static class Command
{
    public record CreateYardPriceCommand(Guid UserId, Request.CreateYardPriceRequest Data)
        : ICommand<Response.YardPriceResponse>;

    public record UpdateYardPriceCommand(Request.UpdateYardPriceRequest Data)
        : ICommand<Response.YardPriceResponse>;

    public record DeleteYardPricesCommand(List<string> Ids)
        : ICommand;
}
