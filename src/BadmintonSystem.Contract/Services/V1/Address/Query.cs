using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.Address;

public static class Query
{
    public record GetAddressByIdQuery(Guid Id)
        : IQuery<Response.AddressResponse>;
}
