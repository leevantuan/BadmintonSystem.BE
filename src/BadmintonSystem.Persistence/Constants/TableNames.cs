namespace BadmintonSystem.Persistence.Constants;

internal static class TableNames
{
    // ******************* Plural Nouns *******************

    internal const string Actions = nameof(Actions);
    internal const string Functions = nameof(Functions);

    internal const string AppUsers = nameof(AppUsers);
    internal const string AppRoles = nameof(AppRoles);
    internal const string AppUserRoles = nameof(AppUserRoles);

    internal const string AppUserClaims = nameof(AppUserClaims); // IdentityUserClaim
    internal const string AppRoleClaims = nameof(AppRoleClaims); // IdentityRoleClaim
    internal const string AppUserLogins = nameof(AppUserLogins); // IdentityUserLogin
    internal const string AppUserTokens = nameof(AppUserTokens); // IdentityUserToken

    // ******************* Singular Nouns *******************
    internal const string Category = nameof(Category);
    internal const string Service = nameof(Service);
    internal const string PaymentMethod = nameof(PaymentMethod);
    internal const string Notification = nameof(Notification);
    internal const string Address = nameof(Address);
    internal const string UserAddress = nameof(UserAddress);
    internal const string ClubAddress = nameof(ClubAddress);
    internal const string Club = nameof(Club);
    internal const string ClubInformation = nameof(ClubInformation);
    internal const string ClubImage = nameof(ClubImage);
    internal const string Yard = nameof(Yard);
    internal const string YardType = nameof(YardType);
    internal const string Review = nameof(Review);
    internal const string ReviewImage = nameof(ReviewImage);
    internal const string BookingTime = nameof(BookingTime);
    internal const string BookingLine = nameof(BookingLine);
    internal const string TimeSlot = nameof(TimeSlot);
    internal const string Booking = nameof(Booking);
    internal const string Sale = nameof(Sale);
    internal const string Price = nameof(Price);
    internal const string YardPrice = nameof(YardPrice);
    internal const string DayOff = nameof(DayOff);
    internal const string DayOfWeek = nameof(DayOfWeek);
    internal const string FixedSchedule = nameof(FixedSchedule);
    internal const string TimeSlotOfWeek = nameof(TimeSlotOfWeek);
    internal const string ServiceLine = nameof(ServiceLine);
    internal const string ComboFixed = nameof(ComboFixed);
    internal const string Bill = nameof(Bill);
    internal const string BillLine = nameof(BillLine);
    internal const string Provider = nameof(Provider);
    internal const string InventoryReceipt = nameof(InventoryReceipt);
    internal const string OriginalQuantity = nameof(OriginalQuantity);
    internal const string ChatRoom = nameof(ChatRoom);
    internal const string ChatMessage = nameof(ChatMessage);

    internal const string Tenant = nameof(Tenant);
    internal const string BookingHistory = nameof(BookingHistory);
}
