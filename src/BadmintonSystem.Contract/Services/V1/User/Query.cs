using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;

namespace BadmintonSystem.Contract.Services.V1.User;

public static class Query
{
    public record RegisterByCustomerQuery(Request.CreateUserAndAddress Data) : IQuery;

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

    // GET BY ID
    public record GetAddressesByIdWithFilterAndSortQuery(
        Guid UserId,
        Abstractions.Shared.Request.PagedFilterAndSortQueryRequest Data)
        : IQuery<Response.AddressByUserDetailResponse>;

    public record GetPaymentMethodsByIdQuery(
        Guid UserId)
        : IQuery<Response.PaymentMethodByUserResponse>;

    public record GetNotificationsByIdWithFilterAndSortQuery(
        Guid UserId,
        Abstractions.Shared.Request.PagedFilterAndSortQueryRequest Data)
        : IQuery<Response.NotificationByUserResponse>;

    public record GetReviewsByIdWithFilterAndSortQuery(
        Guid UserId,
        Abstractions.Shared.Request.PagedFilterAndSortQueryRequest Data)
        : IQuery<Response.ReviewByUserResponse>;
}
