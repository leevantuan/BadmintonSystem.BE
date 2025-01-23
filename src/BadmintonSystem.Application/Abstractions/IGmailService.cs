using BadmintonSystem.Contract.Services.V1.Gmail;

namespace BadmintonSystem.Application.Abstractions;

public interface IGmailService
{
    Task<bool> SendMail(Request.GmailRequest request);

    Task<bool> SendMailAsync(Request.GmailRequest request);

    Task<bool> SendBookingInformationMail(Request.BookingInformationInGmailRequest request);
}
