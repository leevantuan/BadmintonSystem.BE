﻿using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Services.V2.AdditionalService;

namespace BadmintonSystem.Application.UseCases.V2.AdditionalService.Events;
public class SendEmailWhenAdditionalServiceChangedEventHandler
    : IDomainEventHandler<DomainEvent.AdditionalServiceDeleted>,
    IDomainEventHandler<DomainEvent.AdditionalServiceCreated>,
    IDomainEventHandler<DomainEvent.AdditionalServiceUpdated>
{
    public async Task Handle(DomainEvent.AdditionalServiceDeleted notification, CancellationToken cancellationToken)
    {
        SendEmail();
        await Task.Delay(1000);
    }

    public async Task Handle(DomainEvent.AdditionalServiceUpdated notification, CancellationToken cancellationToken)
    {
        SendEmail();
        await Task.Delay(1000);
    }

    public async Task Handle(DomainEvent.AdditionalServiceCreated notification, CancellationToken cancellationToken)
    {
        SendEmail();
        await Task.Delay(1000);
    }

    private void SendEmail()
    {

    }
}