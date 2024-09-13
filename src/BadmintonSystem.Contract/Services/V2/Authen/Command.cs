using BadmintonSystem.Contract.Abstractions.Messages;
using static BadmintonSystem.Contract.Services.V2.Authen.Response;

namespace BadmintonSystem.Contract.Services.V2.Authen;
public static class Command
{
    public record Login(string Email, string Password) : ICommand<Authenticed>;
}
