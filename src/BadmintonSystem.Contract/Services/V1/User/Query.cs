using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using static BadmintonSystem.Contract.Services.V1.User.Response;

namespace BadmintonSystem.Contract.Services.V1.User;

public static class Query
{
    public record RegisterByCustomerQuery(Request.CreateUserAndAddress Data) : IQuery;

    public record VerificationEmailWhenRegisterQuery(string Email) : IQuery;

    public record CancelVerificationEmailWhenRegisterQuery(string Email) : IQuery;

    public record GetUsersQuery(Abstractions.Shared.Request.PagedFilterAndSortQueryRequest Data)
        : IQuery<PagedResult<GetUserInfoResponse>>;

    // GET ALLS
    public record GetAddressesByEmailWithFilterAndSortQuery(
        Guid UserId,
        Abstractions.Shared.Request.PagedFilterAndSortQueryRequest Data)
        : IQuery<PagedResult<Response.AddressByUserDetailResponse>>;

    public record GetPaymentMethodsByUserIdQuery(
        Guid UserId)
        : IQuery<PagedResult<Response.PaymentMethodByUserResponse>>;

    public record GetNotificationsByUserIdWithFilterAndSortQuery(
        Guid UserId,
        Abstractions.Shared.Request.PagedFilterAndSortQueryRequest Data)
        : IQuery<PagedResult<Response.NotificationByUserResponse>>;

    public record GetReviewsByUserIdWithFilterAndSortQuery(
        Guid UserId,
        Abstractions.Shared.Request.PagedFilterAndSortQueryRequest Data)
        : IQuery<PagedResult<Response.ReviewByUserResponse>>;

    public record GetBookingsByUserIdWithFilterAndSortQuery(
        Guid UserId,
        Abstractions.Shared.Request.PagedFilterAndSortQueryRequest Data)
        : IQuery<PagedResult<Response.GetBookingByUserIdResponse>>;

    public record GetChatMessagesByUserIdWithFilterAndSortQuery(
        Guid UserId,
        Abstractions.Shared.Request.PagedFilterAndSortQueryRequest Data)
        : IQuery<PagedResult<Response.GetChatMessageByUserIdResponse>>;
}
