namespace BadmintonSystem.Contract.Abstractions.Services;

public interface ICurrentUserInfoService
{
    Guid? UserId { get; set; }

    string? UserName { get; set; }

    string? UserEmail { get; set; }

    string? PhoneNumber { get; set; }

    Task<bool> SetUserInfo(string email);
}
