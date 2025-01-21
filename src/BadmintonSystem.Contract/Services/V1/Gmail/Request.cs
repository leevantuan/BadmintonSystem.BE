namespace BadmintonSystem.Contract.Services.V1.Gmail;

public static class Request
{
    public class GmailRequest
    {
        public string MailTo { get; set; }

        public string MailSubject { get; set; }

        public string MailBody { get; set; }
    }

    public class BookingInformationInGmailRequest : GmailRequest
    {
        public decimal TotalPrice { get; set; }

        public List<BookingInGmailRequest> BookingLines { get; set; }
    }

    public class BookingInGmailRequest
    {
        public string Name { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public decimal Price { get; set; }
    }
}
