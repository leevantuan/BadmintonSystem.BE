using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Booking;
using BadmintonSystem.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;
using Request = BadmintonSystem.Contract.Services.V1.Gmail.Request;

namespace BadmintonSystem.Application.UseCases.V1.Events.Booking;

public sealed class SendEmailByBookingDoneToStaffEventHandler(
    ApplicationDbContext context,
    ISender sender,
    IGmailService mailService,
    IHttpContextAccessor httpContextAccessor)
    : IDomainEventHandler<DomainEvent.BookingNotificationToStaff>
{
    public async Task Handle(DomainEvent.BookingNotificationToStaff notification, CancellationToken cancellationToken)
    {
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

        var gmailStaffRequest = new Request.BookingInformationInGmailRequest
        {
            MailTo = notification.Email,
            MailSubject =
                $"NOTIFICATION BOOKING SUCCESS - Date: {DateTime.Now.Date:dd/MM/yyyy}",
            MailBody = string.Empty,
            BookingLines = bookingLines,
            FullName = notification.Name,
            TotalPrice = totalPrice
        };

        await mailService.SendBookingInformationMailToStaff(gmailStaffRequest);
    }
}
