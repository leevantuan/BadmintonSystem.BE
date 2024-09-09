namespace BadmintonSystem.Contract.Services.V2.Authen;
public static class Response
{
    public class Authenticed
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
