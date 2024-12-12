namespace BadmintonSystem.Domain.Exceptions;

public static class AuthenticationException
{
    public class CookiesUnauthorizedException : UnauthorizedException
    {
        public CookiesUnauthorizedException(string message)
            : base(message)
        { }
    }

    public class CookiesForbiddenException : ForbiddenException
    {
        public CookiesForbiddenException(string message)
            : base(message)
        { }
    }

    public class AuthorizedForbiddenException : ForbiddenException
    {
        public AuthorizedForbiddenException(string message)
            : base(message)
        { }
    }
}
