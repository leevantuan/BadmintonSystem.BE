using BadmintonSystem.Contract.Abstractions.Messages;
using static BadmintonSystem.Contract.Services.V2.Authen.Response;

namespace BadmintonSystem.Contract.Services.V2.Authen;
public class Query
{
    public record Login(string Email, string Password) : IQuery<Authenticed>;
    public record Token(string? AccessToken, string? RefreshToken) : IQuery<Authenticed>;
}
