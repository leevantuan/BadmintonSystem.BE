using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.User;

public static class Response
{
    // USER DETAIL
    public class AppUserResponse : EntityBase<Guid>
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string PhoneNumber { get; set; }

        public int Gender { get; set; }

        public string AvatarUrl { get; set; }
    }

    // Address by user
    public class AddressByUserDetailResponse : Address.Response.AddressResponse
    {
        public int IsDefault { get; set; }
    }

    public class PaymentMethodByUserResponse : PaymentMethod.Response.PaymentMethodDetailResponse
    {
    }

    public class NotificationByUserResponse : Notification.Response.NotificationDetailResponse
    {
    }

    public class ReviewByUserResponse : Review.Response.ReviewDetailResponse
    {
        public List<ReviewImage.Response.ReviewImageDetailResponse>? ReviewImages { get; set; }
    }

    public class GetChatMessageByUserIdResponse : ChatMessage.Response.GetChatMessageByIdResponse
    {
    }

    public class GetBookingByUserIdResponse : Booking.Response.GetBookingDetailResponse
    {
    }

    public class GetChatMessageByUserIdSqlResponse : ChatMessage.Response.GetChatMessageByIdSqlResponse
    {
    }
}
