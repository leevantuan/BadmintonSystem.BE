using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Booking;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using BadmintonSystem.Persistence.Helpers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Request = BadmintonSystem.Contract.Services.V1.Gmail.Request;

namespace BadmintonSystem.Application.UseCases.V1.Events.Booking;

public sealed class SendEmailByBookingDoneEventHandler(
    ApplicationDbContext context,
    ISender sender,
    IGmailService mailService,
    IHttpContextAccessor httpContextAccessor)
    : IDomainEventHandler<DomainEvent.BookingDone>
{
    public async Task Handle(DomainEvent.BookingDone notification, CancellationToken cancellationToken)
    {
        string? email = httpContextAccessor.HttpContext?.GetCurrentUserEmail();

        if (string.IsNullOrEmpty(email))
        {
            throw new EmailException.EmailNotFoundException();
        }

        Result<Response.GetBookingDetailResponse> result =
            await sender.Send(new Query.GetBookingByIdQuery(notification.Id), cancellationToken);

        var bookingLines = new List<Request.BookingInGmailRequest>();

        foreach (Response.BookingLineDetail item in result.Value.BookingLines)
        {
            bookingLines.Add(new Request.BookingInGmailRequest
            {
                Name = item.YardName ?? string.Empty,
                Price = item.Price
            });
        }

        var gmailRequest = new Request.BookingInformationInGmailRequest
        {
            MailTo = email,
            MailSubject = $"WELCOME TO BADMINTON BOOKING WEB - Date: {result.Value.EffectiveDate.Date:dd/MM/yyyy}",
            MailBody = string.Empty,
            BookingLines = bookingLines
        };

        await mailService.SendMailAsync(gmailRequest);
    }
}
