using BadmintonSystem.Domain.Abstractions.Entities;

namespace BadmintonSystem.Domain.Entities;

public class Club : AuditableEntity<Guid>
{
    public string? Name { get; private set; }
    public string? Code { get; private set; }
    public string? HotLine { get; private set; }
    public string? FacebookPageLink { get; private set; }
    public string? InstagramPageLink { get; private set; }
    public string? MapLink { get; private set; }
    public string? ImageLink { get; private set; }
    public TimeSpan? OpeningTime { get; private set; }
    public TimeSpan? ClosingTime { get; private set; }

    public virtual ICollection<Address> Address { get; private set; }

    public static Club CreateClub(string name, string code, string hotLine, string facebookPageLink,
                                  string instagramLink, string mapLink, string imageLink, TimeSpan opening,
                                  TimeSpan closing)
        => new Club
        {
            Id = Guid.NewGuid(),
            Name = name,
            Code = code,
            HotLine = hotLine,
            FacebookPageLink = facebookPageLink,
            InstagramPageLink = instagramLink,
            MapLink = mapLink,
            ImageLink = imageLink,
            OpeningTime = opening,
            ClosingTime = closing,
            Address = new List<Address>()
        };

    public static Club UpdateClub(string name, string code, string hotLine, string facebookPageLink,
                                  string instagramLink, string mapLink, string imageLink, TimeSpan opening,
                                  TimeSpan closing)
        => new Club
        {
            Name = name,
            Code = code,
            HotLine = hotLine,
            FacebookPageLink = facebookPageLink,
            InstagramPageLink = instagramLink,
            MapLink = mapLink,
            ImageLink = imageLink,
            OpeningTime = opening,
            ClosingTime = closing,
            Address = new List<Address>()
        };
}
