using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.Bill;

public static class Command
{
    public record CreateBillCommand(Guid UserId, Request.CreateBillRequest Data)
        : ICommand;

    public record UpdateBillCommand(Request.UpdateBillRequest Data)
        : ICommand;

    public record DeleteBillCommand(Guid Id)
        : ICommand;

    public record CloseBillCommand(Guid BillId)
        : ICommand;

    public record OpenYardByBillInBookingCommand(Guid BillId)
        : ICommand;

    public record OpenYardByBillCommand(BillLine.Request.CreateBillLineRequest Data)
        : ICommand;

    public record CloseYardByBillCommand(BillLine.Request.UpdateBillLineRequest Data)
        : ICommand;

    // Service
    public record UpdateQuantityServiceByBillCommand(BillLine.Request.UpdateQuantityServiceRequest Data)
        : ICommand;

    public record CreateServicesByBillCommand(Guid BillId, List<ServiceLine.Request.CreateServiceLineRequest> Data)
        : ICommand;

    public record DeleteServiceLineByBillCommand(Guid ServiceLineId)
        : ICommand;
}
