namespace BadmintonSystem.Domain.Entities;
public class ClubAddress
{
    public Guid AddressId { get; set; }

    public Guid ClubId { get; set; }

    public string? Branch { get; set; }
}
