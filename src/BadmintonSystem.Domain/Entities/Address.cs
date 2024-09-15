using BadmintonSystem.Domain.Abstractions.Entities;

namespace BadmintonSystem.Domain.Entities;
public class Address : AuditableEntity<Guid>
{
    public string? Unit { get; private set; }               // Số nhà
    public string? Street { get; private set; }             // Đường
    public string? AddressLine_1 { get; private set; }      // KP / Thôn
    public string? AddressLine_2 { get; private set; }      // Xã
    public string? City { get; private set; }               // Thành Phố / Huyện
    public string? State { get; private set; }              // Huyện

    public static Address CreateAddress(string unit,
                                        string street,
                                        string addressLine_1,
                                        string addressLine_2,
                                        string city,
                                        string state)
    {
        return new Address(unit, street, addressLine_1, addressLine_2, city, state);
    }

    public static Address UpdateAddress(string unit,
                                        string street,
                                        string addressLine_1,
                                        string addressLine_2,
                                        string city,
                                        string state)
    {
        return new Address(unit, street, addressLine_1, addressLine_2, city, state);
    }

    private Address(string unit,
                    string street,
                    string addressLine_1,
                    string addressLine_2,
                    string city,
                    string state)
    {
        Unit = unit;
        Street = street;
        AddressLine_1 = addressLine_1;
        AddressLine_2 = addressLine_2;
        City = city;
        State = state;
    }
}
