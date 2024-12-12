using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.Identity;

public static class Query
{
    public record RegisterQuery(Request.RegisterRequest Data) : IQuery;

    public record LoginQuery(string Email, string Password) : IQuery<Response.Authenticated>;

    public record GetUserAuthorizationByEmailQuery(string Email) : IQuery<List<Response.UserAuthorization>>;

    //public record Token(string? AccessToken, string? RefreshToken) : IQuery<Response.Authenticated>;
}
