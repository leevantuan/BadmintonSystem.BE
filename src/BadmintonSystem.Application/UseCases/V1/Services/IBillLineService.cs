namespace BadmintonSystem.Application.UseCases.V1.Services;

public interface IBillLineService
{
    Task OpenBillLineByBill(Guid yardId, Guid billId, CancellationToken cancellationToken);

    Task CloseBillLineByBill(Guid billLineId, CancellationToken cancellationToken);
}
