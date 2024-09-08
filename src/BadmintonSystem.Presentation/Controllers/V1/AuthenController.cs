using System.Security.Claims;
using Asp.Versioning;
using BadmintonSystem.Contract.Services.V1.Authen;
using BadmintonSystem.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace BadmintonSystem.Presentation.Controllers.V1;

[ApiVersion(1)]
public class AuthenController : ApiController
{
    public AuthenController(ISender sender)
        : base(sender)
    {
    }

    [HttpGet("unauthorized")]
    public IActionResult GetUnauthorized()
    {
        return Unauthorized();
    }

    [HttpGet("unauthorizedV2")]
    public IActionResult GetUnauthorizedV2()
    {
        return Unauthorized();
    }

    [HttpGet("forbidden")]
    public HttpResponseMessage GetForbidden()
    {
        return new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
    }

    [HttpPost("login-cookies")]
    public async Task<IActionResult> LoginCookies([FromBody] Request.LoginUserRequest request)
    {
        // Handle check user
        // Get Role
        // List Permission
        // Hash data test Authorize
        string roleName = string.Empty;

        roleName = request.UserName.EndsWith("Admin") ? "Admin" :
                   request.UserName.EndsWith("Manager") ? "Manager" :
                   "Guest";

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, request.UserName),
            new Claim(ClaimTypes.Role, roleName), // truyền roleName vào
            new Claim("Fullname", "Le Van Tuan"),
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        // Setup properties còn kh sẽ sài mặc định
        // Setup Authen properties
        var authProperties = new AuthenticationProperties
        {
            // Allow Refresh = bool
            // refreshing the authentication session should be allowed
            // Coó cho phép refresh hay không == nên
            AllowRefresh = true,

            // ExpiresUtc = DateTime.UtcNow.AddMiniutes(10)
            // The time at a which the authen tiket expire
            // Value set here averride the ExpireTimeSpan option of
            // CookieAuthenicationOptions set with AddCookies
            // Thời gian hết hạn
            ExpiresUtc = DateTime.UtcNow.AddDays(1),

            // Whether the authentication session is persisted across
            // Multiple request , when used with cookies, controls
            // Whether the cookie is lifetime is absolute matching the
            // Lifetiome of the authentication tiket or session-based
            // Cho phép được lưu qua nhiều request không
            IsPersistent = true,

            //IssuedUtc = <DateTimeOffset>,
            // The time at which the authentication tiket was issued

            //RedirectUri = <string>
            // The full path or absolute URI ti be used as AntiforgeryValidationFailedResult http
            // Rediect response value
        };

        // Đưa vào Cookies
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

        return Ok("Login success with cookies!");
    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return Ok("Logout success!");
    }
}
