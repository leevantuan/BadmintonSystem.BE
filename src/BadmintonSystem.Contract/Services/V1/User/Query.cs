using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.User;

public static class Query
{
    public record RegisterByCustomerQuery(Request.CreateUserAndAddress Data) : IQuery;

    public record GetAddressByEmailQuery(string Email) : IQuery<Response.AddressByUserResponse>;
}
