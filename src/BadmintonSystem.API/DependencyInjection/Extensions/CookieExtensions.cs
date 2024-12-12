using BadmintonSystem.Contract.Constants;

namespace BadmintonSystem.API.DependencyInjection.Extensions;

public static class CookieExtensions
{
    public static void AddCookieAuthenticationConfigurationAPI(this IServiceCollection services)
        => services.AddAuthentication(options =>
                        {
                            //options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme; // "Cookies"
                            //options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme; // "Cookies"
                        })
                    .AddCookie(options =>
                        {
                            options.Cookie = new CookieBuilder()
                            {
                                Name = "democicdcookie",
                                //Domain = "localhost"
                            };
                            options.LoginPath = CookiesRedirectPath.UnauthorizedV1;
                            options.AccessDeniedPath = CookiesRedirectPath.ForbiddenV1;
                            options.LogoutPath = CookiesRedirectPath.LogoutV1;
                        });
}
