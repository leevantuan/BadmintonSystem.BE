using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.Sale;

public static class Command
{
    public record CreateSaleCommand(Guid UserId, Request.CreateSaleRequest Data)
        : ICommand<Response.SaleResponse>;

    public record UpdateSaleCommand(Request.UpdateSaleRequest Data)
        : ICommand<Response.SaleResponse>;

    public record DeleteSalesCommand(List<string> Ids)
        : ICommand;
}
