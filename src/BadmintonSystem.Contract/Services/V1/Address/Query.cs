using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.Address;

public static class Query
{
    public record GetAddressesByIdQuery(
        Guid UserId,
        Guid AddressId)
        : IQuery<User.Response.AddressByUserDetailResponse>;
}
