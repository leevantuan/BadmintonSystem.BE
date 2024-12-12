namespace BadmintonSystem.Contract.Services.V1.Identity;

public static class Response
{
    public class Authenticated
    {
        public string? AccessToken { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }
    }

    public class UserDetailResponse
    {
        public User.Response.AppUserResponse User { get; set; }

        public List<UserAuthorization> Authorizations { get; set; }
    }

    public class UserAuthorization
    {
        public string FunctionKey { get; set; }

        public List<string> Action { get; set; }
    }
}
