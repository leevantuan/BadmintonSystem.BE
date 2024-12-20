namespace BadmintonSystem.Contract.Services.V1.User;

public static class Request
{
    public class UserRequest
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public int Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? AvatarUrl { get; set; }
    }

    public class CreateUserAndAddress : UserRequest
    {
        public string? Unit { get; set; }

        public string? Street { get; set; }

        public string? AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }

        public string City { get; set; }

        public string Province { get; set; }
    }

    public class PasswordRequest
    {
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }

    public class ForgetPasswordRequest : PasswordRequest
    {
        public string Email { get; set; }
    }

    public class ChangePasswordRequest : PasswordRequest
    {
        public Guid Id { get; set; }

        public string CurrentPassword { get; set; }
    }

    public class UpdateAddressByUserIdRequest : Address.Request.UpdateAddressRequest
    {
        public int IsDefault { get; set; }
    }

    public class CreateReviewByUserIdRequest : Review.Request.CreateReviewRequest
    {
        public List<ReviewImage.Request.CreateReviewImageRequest>? ReviewImages { get; set; }
    }

    public class UpdateReviewByUserIdRequest : Review.Request.CreateReviewRequest
    {
        public Guid Id { get; set; }

        public List<ReviewImage.Request.CreateReviewImageRequest>? ReviewImages { get; set; }
    }
}
