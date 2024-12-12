namespace BadmintonSystem.Contract.Constants;
public static class CookiesRedirectPath
{
    public const string ApiVersion = "3";
    public const string AuthenticationEndpointRootV1 = $"/api/v{ApiVersion}/Authentications";

    public const string UnauthorizedV1 = $"{AuthenticationEndpointRootV1}/Unauthorized";
    public const string ForbiddenV1 = $"{AuthenticationEndpointRootV1}/Forbidden";
    public const string LogoutV1 = $"{AuthenticationEndpointRootV1}/Logout";
}
