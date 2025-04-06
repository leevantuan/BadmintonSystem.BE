using BadmintonSystem.Contract.Abstractions.Services;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Infrastructure.Services;

public class CurrentUserInfoService(ApplicationDbContext context)
    : ICurrentUserInfoService
{
    public Guid? UserId { get; set; }

    public string? UserName { get; set; }

    public string? UserEmail { get; set; }

    public string? PhoneNumber { get; set; }

    public async Task<bool> SetUserInfo(string email)
    {
        var userInfo = await context.AppUsers.FirstOrDefaultAsync(x => x.Email.Trim().ToLower() == email.Trim().ToLower());
        if (userInfo != null)
        {
            UserId = userInfo.Id;
            UserEmail = userInfo.Email;
            UserName = userInfo.UserName;
            PhoneNumber = userInfo.PhoneNumber;
        }
        else
        {
            throw new Exception("Email invalid");
        }

        return true;
    }
}
