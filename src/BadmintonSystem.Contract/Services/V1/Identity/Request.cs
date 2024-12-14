namespace BadmintonSystem.Contract.Services.V1.Identity;

public static class Request
{
    public record CreateActionRequest(string Name);

    public class UpdateActionRequest
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public int? SortOrder { get; set; }

        public bool? IsActive { get; set; }
    }

    public record CreateFunctionRequest(
        string Name,
        string Url,
        int? ParentId,
        string? CssClass,
        string? Key,
        int ActionValue);

    public class UpdateFunctionRequest
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public string? Url { get; set; }

        public int? ParentId { get; set; }

        public string? CssClass { get; set; }

        public string? Key { get; set; }

        public int? ActionValue { get; set; }
    }

    public record CreateAppRoleRequest(string RoleCode, string? Description);

    public record CreateAppUserRequest(
        string Email,
        string UserName,
        string FirstName,
        string LastName,
        string Password,
        string RoleCode);

    public class UpdateAppRoleRequest
    {
        public Guid? Id { get; set; }

        public string? RoleCode { get; set; }

        public string? Description { get; set; }
    }

    public record RegisterRequest(string Email, string UserName, string FirstName, string LastName, string Password);

    public record CreateAppRoleClaimRequest(string RoleName, string FunctionKey, int ActionValue);

    public record UpdateRoleMultipleForUserRequest(string Email, List<string> Roles);

    public class UpdateAppRoleClaimRequest
    {
        public string RoleCode { get; set; }

        public List<ClaimRequest>? ListFunctions { get; set; }
    }

    public class UpdateAppUserClaimRequest
    {
        public string Email { get; set; }

        public List<ClaimRequest>? ListFunctions { get; set; }
    }

    public class ClaimRequest
    {
        public string FunctionKey { get; set; }

        public string ActionValues { get; set; }
    }

    public record ResetUserToDefaultRole(string Email, string RoleName);

    public class ResetPasswordById : User.Request.PasswordRequest
    {
        public Guid Id { get; set; }
    }
}
