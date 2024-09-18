using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Services.V2.Category;

namespace BadmintonSystem.Application.UseCases.V2.Category.Events;
public class SendSMSWhenCategoryChangedEventHandler
    : IDomainEventHandler<DomainEvent.CategoryDeleted>,
    IDomainEventHandler<DomainEvent.CategoryCreated>,
    IDomainEventHandler<DomainEvent.CategoryUpdated>
{
    public async Task Handle(DomainEvent.CategoryDeleted notification, CancellationToken cancellationToken)
    {
        SendSMS();
        await Task.Delay(1000);
    }

    public async Task Handle(DomainEvent.CategoryUpdated notification, CancellationToken cancellationToken)
    {
        SendSMS();
        await Task.Delay(1000);
    }

    public async Task Handle(DomainEvent.CategoryCreated notification, CancellationToken cancellationToken)
    {
        SendSMS();
        await Task.Delay(1000);
    }

    private void SendSMS()
    {

    }
}
