﻿using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Services.V2.Gender;

namespace BadmintonSystem.Application.UseCases.V2.Events;
public class SendEmailWhenGenderChangedEventHandler
    : IDomainEventHandler<DomainEvent.GenderDeleted>,
    IDomainEventHandler<DomainEvent.GenderCreated>,
    IDomainEventHandler<DomainEvent.GenderUpdated>
{
    public async Task Handle(DomainEvent.GenderDeleted notification, CancellationToken cancellationToken)
    {
        SendEmail();
        await Task.Delay(1000);
    }

    public async Task Handle(DomainEvent.GenderUpdated notification, CancellationToken cancellationToken)
    {
        SendEmail();
        await Task.Delay(1000);
    }

    public async Task Handle(DomainEvent.GenderCreated notification, CancellationToken cancellationToken)
    {
        SendEmail();
        await Task.Delay(1000);
    }

    private void SendEmail()
    {

    }
}
