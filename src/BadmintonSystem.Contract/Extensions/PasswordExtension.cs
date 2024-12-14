namespace BadmintonSystem.Contract.Extensions;

public static class PasswordExtension
{
    public static bool ConfirmPassword(string password, string confirmPassword)
    {
        return password == confirmPassword;
    }
}
