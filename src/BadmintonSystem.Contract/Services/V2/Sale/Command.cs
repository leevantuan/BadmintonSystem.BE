using BadmintonSystem.Contract.Abstractions.Messages;

namespace BadmintonSystem.Contract.Services.V2.Sale;
public static class Command
{
    public record CreateSaleCommand(Request.SaleRequest Data) : ICommand;
    public record UpdateSaleCommand(Guid Id, string Name, decimal Persent, DateTime StartTime, DateTime EndTime, bool? Status = false) : ICommand;
    public record DeleteSaleCommand(Guid Id) : ICommand;
}
