using System.Text;
using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Contract.Services.V1.Gmail;
using BadmintonSystem.Contract.Source;
using BadmintonSystem.Infrastructure.DependencyInjection.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

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

    private static string GenerateBookingDetailInformationHTML(List<Request.BookingInGmailRequest> bookings)
    {
        var bookingDetailHtml = new StringBuilder();

        foreach (Request.BookingInGmailRequest booking in bookings)
        {
            bookingDetailHtml.AppendLine("<tr>");
            bookingDetailHtml.AppendLine("<td><strong style='color: blue;'>Ngày nhận chỗ:</strong></td>");
            bookingDetailHtml.AppendLine($"<td>{booking.EffectiveDate}</td>");
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
                bookingDetailHtml.AppendLine($"<td style='color: red;'>{yard.Price} vnđ</td>");
                bookingDetailHtml.AppendLine("</tr>");
            }
        }

        return bookingDetailHtml.ToString();
    }

    // public async Task<bool> TestSendBookingInformationMail(Request.BookingInformationInGmailRequest request)
    // {
    //     try
    //     {
    //         using (var emailMessage = new MimeMessage())
    //         {
    //             var emailFrom = new MailboxAddress(_mailOption.SenderName, _mailOption.SenderEmail);
    //             emailMessage.From.Add(emailFrom);
    //
    //             emailMessage.To.Add(MailboxAddress.Parse(request.MailTo));
    //
    //             emailMessage.Subject = request.MailSubject;
    //
    //             string templateUrl =
    //                 "https://res.cloudinary.com/dldksrtdf/raw/upload/v1736184430/OrderTemplate_onv5bo.html";
    //
    //             using (var client = new HttpClient())
    //             {
    //                 string emailTemplateText = await client.GetStringAsync(templateUrl);
    //
    //                 emailTemplateText =
    //                     emailTemplateText.Replace("{DateTime}", DateTime.Now.ToString("MM/dd/yyyy HH:mm tt"));
    //
    //                 string bookingDetailHtml = GenerateOrderDetailInformationHTML(request.BookingLines);
    //
    //                 emailTemplateText = emailTemplateText.Replace("{ProductList}", bookingDetailHtml);
    //
    //                 var emailBodyBuilder = new BodyBuilder
    //                 {
    //                     HtmlBody = emailTemplateText,
    //                     TextBody = "Plain Text goes here to avoid marked as spam for some email servers."
    //                 };
    //
    //                 emailMessage.Body = emailBodyBuilder.ToMessageBody();
    //
    //                 using (var mailClient = new SmtpClient())
    //                 {
    //                     mailClient.Connect(_mailOption.Server, _mailOption.Port, SecureSocketOptions.StartTls);
    //                     mailClient.Authenticate(_mailOption.SenderEmail, _mailOption.Password);
    //                     mailClient.Send(emailMessage);
    //                     mailClient.Disconnect(true);
    //                 }
    //             }
    //         }
    //
    //         return true;
    //     }
    //     catch (Exception ex)
    //     {
    //         throw new NotImplementedException(ex.Message);
    //     }
    // }
}
