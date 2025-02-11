namespace BadmintonSystem.Domain.Exceptions;

public static class IdentityException
{
    public class ActionNotFoundException : NotFoundException
    {
        public ActionNotFoundException(int actionId)
            : base($"Action with the id {actionId} was not found")
        {
        }
    }

    public sealed class ActionAlreadyExistException : AlreadyExistException
    {
        public ActionAlreadyExistException(string actionName)
            : base($"Action with the name {actionName} was existed")
        {
        }
    }

    public sealed class FunctionAlreadyExistException : AlreadyExistException
    {
        public FunctionAlreadyExistException(string functionName)
            : base($"Function with the name {functionName} was existed")
        {
        }
    }

    public sealed class AppRoleAlreadyExistException : AlreadyExistException
    {
        public AppRoleAlreadyExistException(string roleCode)
            : base($"Role with the code {roleCode} was existed")
        {
        }
    }

    public class AppRoleNotFoundException : NotFoundException
    {
        public AppRoleNotFoundException(string roleName)
            : base($"Role with the name {roleName} was not found")
        {
        }
    }

    public sealed class AppRoleException : BadRequestException
    {
        public AppRoleException(string errors)
            : base(errors)
        {
        }
    }

    public sealed class AppUserNotFoundException : NotFoundException
    {
        public AppUserNotFoundException(string email)
            : base($"User with the email {email} was not found")
        {
        }

        public AppUserNotFoundException(Guid userId)
            : base($"User with the id {userId} was not found")
        {
        }
    }

    public sealed class AppUserAlreadyExistException : AlreadyExistException
    {
        public AppUserAlreadyExistException(string email)
            : base($"User with the email {email} was existed")
        {
        }
    }

    public sealed class AppUserException : BadRequestException
    {
        public AppUserException(string errors)
            : base(errors)
        {
        }
    }

    public sealed class AppUserPasswordConditionException : BadRequestException
    {
        public AppUserPasswordConditionException(string errors)
            : base(errors)
        {
        }
    }
}
