using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.Price;

public static class Command
{
    public record CreatePriceCommand(Guid UserId, Request.CreatePriceRequest Data)
        : ICommand<Response.PriceResponse>;

    public record UpdatePriceCommand(Request.UpdatePriceRequest Data)
        : ICommand<Response.PriceResponse>;

    public record DeletePricesCommand(List<string> Ids)
        : ICommand;
}
