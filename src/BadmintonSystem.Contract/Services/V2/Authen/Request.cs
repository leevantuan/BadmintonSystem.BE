namespace BadmintonSystem.Contract.Services.V2.Authen;
public static class Request
{
    public record LoginUserRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

}
