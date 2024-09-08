using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Services.Gender;

namespace BadmintonSystem.Application.UseCases.V1.Events;
public class SendSMSWhenGenderChangedEnventHandler
    : IDomainEventHandler<DomainEvent.GenderDeleted>,
    IDomainEventHandler<DomainEvent.GenderCreated>,
    IDomainEventHandler<DomainEvent.GenderUpdated>
{
    public async Task Handle(DomainEvent.GenderDeleted notification, CancellationToken cancellationToken)
    {
        SendSMS();
        await Task.Delay(10000);
    }

    public async Task Handle(DomainEvent.GenderUpdated notification, CancellationToken cancellationToken)
    {
        SendSMS();
        await Task.Delay(10000);
    }

    public async Task Handle(DomainEvent.GenderCreated notification, CancellationToken cancellationToken)
    {
        SendSMS();
        await Task.Delay(10000);
    }

    private void SendSMS()
    {

    }
}
