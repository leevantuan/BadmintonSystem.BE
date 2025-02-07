using BadmintonSystem.Contract.Services.V1.User;

namespace BadmintonSystem.Application.Abstractions;

public interface IRegisterHub
{
    Task VerificationEmailAsync(Response.VerifyResponseHub message);
}
