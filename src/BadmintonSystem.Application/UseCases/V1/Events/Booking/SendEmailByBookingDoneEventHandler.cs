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

        string? userName = httpContextAccessor.HttpContext?.GetCurrentUserName();

        if (string.IsNullOrEmpty(email))
        {
            throw new EmailException.EmailNotFoundException();
        }

        var results = new List<Result<Response.GetBookingDetailResponse>>();

        foreach (Guid id in notification.Ids)
        {
            Result<Response.GetBookingDetailResponse> result =
                await sender.Send(new Query.GetBookingByIdQuery(id), cancellationToken);

            results.Add(result);
        }

        var bookingLines = new List<Request.BookingInGmailRequest>();

        foreach (Result<Response.GetBookingDetailResponse> item in results)
        {
            bookingLines.Add(new Request.BookingInGmailRequest
            {
                EffectiveDate = item.Value.EffectiveDate,

                Yards = item.Value.BookingLines.Select(x => new Request.YardDetailInGmail
                {
                    Name = x.YardName,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    Price = x.Price
                }).ToList()
            });
        }

        decimal totalPrice = bookingLines.Sum(bl => bl.Yards.Sum(yard => yard.Price));


        var gmailRequest = new Request.BookingInformationInGmailRequest
        {
            MailTo = email,
            MailSubject = $"WELCOME TO BADMINTON BOOKING WEB - Date: {DateTime.Now.Date:dd/MM/yyyy}",
            MailBody = string.Empty,
            BookingLines = bookingLines,
            FullName = userName,
            TotalPrice = totalPrice
        };

        await mailService.SendBookingInformationMail(gmailRequest);
    }
}
