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

        public string FullName { get; set; }

        public List<BookingInGmailRequest> BookingLines { get; set; }
    }

    public class BookingInGmailRequest
    {
        public DateTime EffectiveDate { get; set; }

        public List<YardDetailInGmail> Yards { get; set; }
    }

    public class  YardDetailInGmail
    {
        public string Name { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public decimal Price { get; set; }
    }
}
