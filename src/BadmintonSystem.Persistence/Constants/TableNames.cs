namespace BadmintonSystem.Persistence.Constants;
internal static class TableNames
{
    // *********** Plural Nouns ***********
    internal const string Actions = nameof(Actions);
    internal const string Functions = nameof(Functions);
    internal const string ActionInFunctions = nameof(ActionInFunctions);
    internal const string PermissionInRoles = nameof(PermissionInRoles);
    internal const string PermissionInUsers = nameof(PermissionInUsers);
    //internal const string Permissions = nameof(Permissions);

    internal const string AppUsers = nameof(AppUsers);
    internal const string AppRoles = nameof(AppRoles);
    internal const string AppUserRoles = nameof(AppUserRoles);

    internal const string AppUserClaims = nameof(AppUserClaims); // IdentityUserClaim
    internal const string AppRoleClaims = nameof(AppRoleClaims); // IdentityRoleClaim
    internal const string AppUserLogins = nameof(AppUserLogins); // IdentityRoleClaim
    internal const string AppUserTokens = nameof(AppUserTokens); // IdentityUserToken

    // *********** Singular Nouns ***********
    internal const string Gender = nameof(Gender);
    internal const string Address = nameof(Address);
    internal const string Category = nameof(Category);
    internal const string AdditionalService = nameof(AdditionalService);
    internal const string Club = nameof(Club);
    internal const string Sale = nameof(Sale);
    internal const string UserAddress = nameof(UserAddress);
}
