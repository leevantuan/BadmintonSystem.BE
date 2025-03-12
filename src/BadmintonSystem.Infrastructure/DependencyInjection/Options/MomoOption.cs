namespace BadmintonSystem.Infrastructure.DependencyInjection.Options;

public class MomoOption
{
    public string PartnerCode { get; set; }

    public string AccessKey { get; set; }

    public string SecretKey { get; set; }

    public string PaymentUrl { get; set; }

    public string ReturnUrl { get; set; }

    public string IpnUrl { get; set; }

    public string NotifyUrl { get; set; }
}
