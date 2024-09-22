using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Services.V2.Club;

namespace BadmintonSystem.Application.UseCases.V2.Club.Events;
public class SendEmailWhenClubChangedEventHandler
    : IDomainEventHandler<DomainEvent.ClubDeleted>,
    IDomainEventHandler<DomainEvent.ClubCreated>,
    IDomainEventHandler<DomainEvent.ClubUpdated>
{
    public async Task Handle(DomainEvent.ClubDeleted notification, CancellationToken cancellationToken)
    {
        SendEmail();
        await Task.Delay(1000);
    }

    public async Task Handle(DomainEvent.ClubUpdated notification, CancellationToken cancellationToken)
    {
        SendEmail();
        await Task.Delay(1000);
    }

    public async Task Handle(DomainEvent.ClubCreated notification, CancellationToken cancellationToken)
    {
        SendEmail();
        await Task.Delay(1000);
    }

    private void SendEmail()
    {

    }
}
