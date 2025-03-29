using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.Identity;

public static class Command
{
    public record CreateActionCommand(Request.CreateActionRequest Data) : ICommand;

    public record CreateFunctionCommand(Request.CreateFunctionRequest Data) : ICommand;

    public record CreateAppRoleCommand(Request.CreateAppRoleRequest Data) : ICommand;

    public record CreateAppUserCommand(Request.CreateAppUserRequest Data) : ICommand;

    public record CreateAppRoleClaimCommand(Request.CreateAppRoleClaimRequest Data) : ICommand;

    public record UpdateRoleMultipleForUserCommand(Request.UpdateRoleMultipleForUserRequest Data) : ICommand;

    public record UpdateAppRoleClaimCommand(Request.UpdateAppRoleClaimRequest Data) : ICommand;

    public record UpdateAppUserClaimCommand(Request.UpdateAppUserClaimRequest Data) : ICommand;

    public record ResetUserToDefaultRoleCommand(Request.ResetUserToDefaultRole Data) : ICommand;

    public record ResetPasswordByIdCommand(Request.ResetPasswordById Data) : ICommand;
}
