using System.Text;
using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Contract.Services.V1.Gmail;
using BadmintonSystem.Contract.Source;
using BadmintonSystem.Infrastructure.DependencyInjection.Options;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace BadmintonSystem.Infrastructure.Services;

public class GmailService : IGmailService
{
    private readonly MailOption _mailOption = new();

    public GmailService(IConfiguration configuration)
    {
        configuration.GetSection(nameof(MailOption)).Bind(_mailOption);
    }

    public Task<bool> SendMail(Request.GmailRequest request)
    {
        try
        {
            using (var emailMessage = new MimeMessage())
            {
                var emailFrom = new MailboxAddress(_mailOption.SenderName, _mailOption.SenderEmail);
                emailMessage.From.Add(emailFrom);

                // emailMessage.To.Add(MailboxAddress.Parse(request.MailTo));
                emailMessage.To.Add(MailboxAddress.Parse("levantuan02022002@gmail.com"));

                emailMessage.Subject = request.MailSubject;

                var emailBodyBuilder = new BodyBuilder
                {
                    TextBody = request.MailBody
                };

                emailMessage.Body = emailBodyBuilder.ToMessageBody();
                using (var mailClient = new SmtpClient())
                {
                    mailClient.Connect(_mailOption.Server, _mailOption.Port, SecureSocketOptions.StartTls);
                    mailClient.Authenticate(_mailOption.UserName, _mailOption.Password);
                    mailClient.Send(emailMessage);
                    mailClient.Disconnect(true);
                }
            }

            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            throw new NotImplementedException(ex.Message);
        }
    }

    public async Task<bool> SendMailAsync(Request.GmailRequest request)
    {
        try
        {
            using (var emailMessage = new MimeMessage())
            {
                var emailFrom = new MailboxAddress(_mailOption.SenderName, _mailOption.SenderEmail);
                emailMessage.From.Add(emailFrom);

                emailMessage.To.Add(MailboxAddress.Parse(request.MailTo));

                emailMessage.Subject = request.MailSubject;

                var emailBodyBuilder = new BodyBuilder
                {
                    TextBody = request.MailBody
                };

                emailMessage.Body = emailBodyBuilder.ToMessageBody();
                using (var mailClient = new SmtpClient())
                {
                    await mailClient.ConnectAsync(_mailOption.Server, _mailOption.Port, SecureSocketOptions.StartTls);
                    await mailClient.AuthenticateAsync(_mailOption.SenderEmail, _mailOption.Password);
                    await mailClient.SendAsync(emailMessage);
                    await mailClient.DisconnectAsync(true);
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new NotImplementedException(ex.Message);
        }
    }

    public async Task<bool> SendBookingInformationMail(Request.BookingInformationInGmailRequest request)
    {
        try
        {
            using (var emailMessage = new MimeMessage())
            {
                var emailFrom = new MailboxAddress(_mailOption.SenderName, _mailOption.SenderEmail);
                emailMessage.From.Add(emailFrom);

                emailMessage.To.Add(MailboxAddress.Parse(request.MailTo));

                emailMessage.Subject = request.MailSubject;

                string emailTemplateText = string.Empty;

                // emailTemplateText =
                //     emailTemplateText.Replace("{DateTime}", DateTime.Now.ToString("MM/dd/yyyy HH:mm tt"));

                string bookingDetailHtml = GenerateBookingDetailInformationHTML(request.BookingLines);

                emailTemplateText = TemplateEmail.EmailTemplate.GetBookingConfirmationEmail(request.FullName,
                    request.TotalPrice,
                    bookingDetailHtml);

                var emailBodyBuilder = new BodyBuilder
                {
                    HtmlBody = emailTemplateText,
                    TextBody = "Plain Text goes here to avoid marked as spam for some email servers."
                };

                emailMessage.Body = emailBodyBuilder.ToMessageBody();

                using (var mailClient = new SmtpClient())
                {
                    await mailClient.ConnectAsync(_mailOption.Server, _mailOption.Port, SecureSocketOptions.StartTls);
                    await mailClient.AuthenticateAsync(_mailOption.SenderEmail, _mailOption.Password);
                    await mailClient.SendAsync(emailMessage);
                    await mailClient.DisconnectAsync(true);
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new NotImplementedException(ex.Message);
        }
    }

    public async Task<bool> SendBookingInformationMailToStaff(Request.BookingInformationInGmailRequest request)
    {
        try
        {
            using (var emailMessage = new MimeMessage())
            {
                var emailFrom = new MailboxAddress(_mailOption.SenderName, _mailOption.SenderEmail);
                emailMessage.From.Add(emailFrom);

                emailMessage.To.Add(emailFrom);

                emailMessage.Subject = request.MailSubject;

                string emailTemplateText = string.Empty;

                string bookingDetailHtml = GenerateBookingDetailInformationHTML(request.BookingLines);

                emailTemplateText = TemplateEmail.EmailTemplate.GetBookingConfirmationEmailToStaff(request.FullName,
                    request.TotalPrice,
                    bookingDetailHtml);

                var emailBodyBuilder = new BodyBuilder
                {
                    HtmlBody = emailTemplateText,
                    TextBody = "Plain Text goes here to avoid marked as spam for some email servers."
                };

                emailMessage.Body = emailBodyBuilder.ToMessageBody();

                using (var mailClient = new SmtpClient())
                {
                    await mailClient.ConnectAsync(_mailOption.Server, _mailOption.Port, SecureSocketOptions.StartTls);
                    await mailClient.AuthenticateAsync(_mailOption.SenderEmail, _mailOption.Password);
                    await mailClient.SendAsync(emailMessage);
                    await mailClient.DisconnectAsync(true);
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new NotImplementedException(ex.Message);
        }
    }

    public async Task SendVerificationEmailAsync(string email, string verificationLink)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("BOOKING WEB", _mailOption.SenderEmail));
            message.To.Add(new MailboxAddress("Recipient Name", email));
            message.Subject = "Email Verification";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody =
                    $"Please verify your email by clicking the following link: <a href='{verificationLink}'>Verify Email</a>",
                TextBody = $"Please verify your email by clicking the following link: {verificationLink}"
            };

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_mailOption.Server, _mailOption.Port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_mailOption.UserName, _mailOption.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Email erorr: {ex.Message}.");
        }
    }

    private static string GenerateBookingDetailInformationHTML(List<Request.BookingInGmailRequest> bookings)
    {
        var bookingDetailHtml = new StringBuilder();

        foreach (Request.BookingInGmailRequest booking in bookings)
        {
            string date = booking.EffectiveDate.ToString("dd/MM/yyyy");

            bookingDetailHtml.AppendLine("<tr>");
            bookingDetailHtml.AppendLine("<td><strong style='color: blue;'>Ngày nhận chỗ:</strong></td>");
            bookingDetailHtml.AppendLine($"<td>{date}</td>");
            bookingDetailHtml.AppendLine("</tr>");

            foreach (Request.YardDetailInGmail yard in booking.Yards)
            {
                bookingDetailHtml.AppendLine("<tr>");
                bookingDetailHtml.AppendLine("<td><strong>Sân:</strong></td>");
                bookingDetailHtml.AppendLine($"<td>{yard.Name}</td>");
                bookingDetailHtml.AppendLine("</tr>");

                bookingDetailHtml.AppendLine("<tr>");
                bookingDetailHtml.AppendLine("<td><strong>Thời gian:</strong></td>");
                bookingDetailHtml.AppendLine($"<td>{yard.StartTime} - {yard.EndTime}</td>");
                bookingDetailHtml.AppendLine("</tr>");
                bookingDetailHtml.AppendLine("</tr>");

                bookingDetailHtml.AppendLine("<tr>");
                bookingDetailHtml.AppendLine("<td><strong>Giá:</strong></td>");
                bookingDetailHtml.AppendLine($"<td style='color: red;'>{yard.Price:n0} VND</td>");
                bookingDetailHtml.AppendLine("</tr>");
            }
        }

        return bookingDetailHtml.ToString();
    }
}
