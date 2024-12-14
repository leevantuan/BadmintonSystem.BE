using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.User;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Enumerations;
using static BadmintonSystem.Contract.Services.V1.Category.Response;
using Request = BadmintonSystem.Contract.Services.V1.Category.Request;
using V1 = BadmintonSystem.Contract.Services.V1;

namespace BadmintonSystem.Application.Mappers;

public class ServiceV1Profile : Profile
{
    public ServiceV1Profile()
    {
        // App User
        CreateMap<AppUser, Response.AppUserResponse>().ReverseMap();

        // Address
        CreateMap<Address, V1.Address.Response.AddressResponse>().ReverseMap();
        CreateMap<Address, V1.Address.Request.CreateAddressRequest>().ReverseMap();
        // CreateMap<PagedResult<Service>, PagedResult<V1.Service.Response.ServiceResponse>>().ReverseMap();
        // CreateMap<Service, V1.Service.Response.ServiceDetailResponse>().ReverseMap();
        // CreateMap<PagedResult<Service>, PagedResult<V1.Service.Response.ServiceDetailResponse>>()
        //     .ForMember(dest => dest.Items, opt
        //         => opt.MapFrom(src => src.Items)).ReverseMap();

        // Category ==> Có lớp con
        CreateMap<Category, CategoryResponse>().ReverseMap();
        CreateMap<Category, Request.CreateCategoryRequest>().ReverseMap();
        CreateMap<PagedResult<Category>, PagedResult<CategoryResponse>>().ReverseMap();
        CreateMap<Category, CategoryDetailResponse>()
            .ForMember(dest => dest.Services, opt
                => opt.MapFrom(src => src.Services));
        CreateMap<PagedResult<Category>, PagedResult<CategoryDetailResponse>>()
            .ForMember(dest => dest.Items, opt
                => opt.MapFrom(src => src.Items));

        // Service ==> Không có lớp con
        CreateMap<Service, V1.Service.Response.ServiceResponse>().ReverseMap();
        CreateMap<Service, V1.Service.Request.CreateServiceRequest>().ReverseMap();
        CreateMap<PagedResult<Service>, PagedResult<V1.Service.Response.ServiceResponse>>().ReverseMap();
        CreateMap<Service, V1.Service.Response.ServiceDetailResponse>().ReverseMap();
        CreateMap<PagedResult<Service>, PagedResult<V1.Service.Response.ServiceDetailResponse>>()
            .ForMember(dest => dest.Items, opt
                => opt.MapFrom(src => src.Items)).ReverseMap();

        // Sale
        CreateMap<Sale, V1.Sale.Response.SaleResponse>().ReverseMap();
        CreateMap<V1.Sale.Request.CreateSaleRequest, Sale>()
            .ForMember(dest => dest.IsActive, opt
                => opt.MapFrom(src => (ActiveEnum)src.IsActive));
        CreateMap<PagedResult<Sale>, PagedResult<V1.Sale.Response.SaleResponse>>().ReverseMap();
        CreateMap<Sale, V1.Sale.Response.SaleDetailResponse>().ReverseMap();
        CreateMap<PagedResult<Sale>, PagedResult<V1.Sale.Response.SaleDetailResponse>>()
            .ForMember(dest => dest.Items, opt
                => opt.MapFrom(src => src.Items)).ReverseMap();

        // Yard Type
        CreateMap<YardType, V1.YardType.Response.YardTypeResponse>().ReverseMap();
        CreateMap<YardType, V1.YardType.Request.CreateYardTypeRequest>().ReverseMap();
        CreateMap<PagedResult<YardType>, PagedResult<V1.YardType.Response.YardTypeResponse>>().ReverseMap();
        CreateMap<YardType, V1.YardType.Response.YardTypeDetailResponse>().ReverseMap();
        CreateMap<PagedResult<YardType>, PagedResult<V1.YardType.Response.YardTypeDetailResponse>>()
            .ForMember(dest => dest.Items, opt
                => opt.MapFrom(src => src.Items)).ReverseMap();

        // Yard
        CreateMap<Yard, V1.Yard.Response.YardResponse>().ReverseMap();
        CreateMap<Yard, V1.Yard.Request.CreateYardRequest>().ReverseMap();
        CreateMap<PagedResult<Yard>, PagedResult<V1.Yard.Response.YardResponse>>().ReverseMap();
        CreateMap<Yard, V1.Yard.Response.YardDetailResponse>().ReverseMap();
        CreateMap<PagedResult<Yard>, PagedResult<V1.Yard.Response.YardDetailResponse>>()
            .ForMember(dest => dest.Items, opt
                => opt.MapFrom(src => src.Items)).ReverseMap();

        // Review
        CreateMap<Review, V1.Review.Response.ReviewResponse>().ReverseMap();
        CreateMap<Review, V1.Review.Request.CreateReviewRequest>().ReverseMap();
        CreateMap<PagedResult<Review>, PagedResult<V1.Review.Response.ReviewResponse>>().ReverseMap();
        CreateMap<Review, V1.Review.Response.ReviewDetailResponse>().ReverseMap();
        CreateMap<PagedResult<Review>, PagedResult<V1.Review.Response.ReviewDetailResponse>>()
            .ForMember(dest => dest.Items, opt
                => opt.MapFrom(src => src.Items)).ReverseMap();

        // Notification
        CreateMap<Notification, V1.Notification.Response.NotificationResponse>().ReverseMap();
        CreateMap<Notification, V1.Notification.Request.CreateNotificationRequest>().ReverseMap();
        CreateMap<PagedResult<Notification>, PagedResult<V1.Notification.Response.NotificationResponse>>().ReverseMap();
        CreateMap<Notification, V1.Notification.Response.NotificationDetailResponse>().ReverseMap();
        CreateMap<PagedResult<Notification>, PagedResult<V1.Notification.Response.NotificationDetailResponse>>()
            .ForMember(dest => dest.Items, opt
                => opt.MapFrom(src => src.Items)).ReverseMap();

        // Time Slot
        CreateMap<TimeSlot, V1.TimeSlot.Response.TimeSlotResponse>().ReverseMap();
        CreateMap<TimeSlot, V1.TimeSlot.Request.CreateTimeSlotRequest>().ReverseMap();
        CreateMap<PagedResult<TimeSlot>, PagedResult<V1.TimeSlot.Response.TimeSlotResponse>>().ReverseMap();
        CreateMap<TimeSlot, V1.TimeSlot.Response.TimeSlotDetailResponse>().ReverseMap();
        CreateMap<PagedResult<TimeSlot>, PagedResult<V1.TimeSlot.Response.TimeSlotDetailResponse>>()
            .ForMember(dest => dest.Items, opt
                => opt.MapFrom(src => src.Items)).ReverseMap();

        // Club
        CreateMap<Club, V1.Club.Response.ClubResponse>().ReverseMap();
        CreateMap<Club, V1.Club.Request.CreateClubRequest>().ReverseMap();
        CreateMap<PagedResult<Club>, PagedResult<V1.Club.Response.ClubResponse>>().ReverseMap();
        CreateMap<Club, V1.Club.Response.ClubDetailResponse>().ReverseMap();
        CreateMap<PagedResult<Club>, PagedResult<V1.Club.Response.ClubDetailResponse>>()
            .ForMember(dest => dest.Items, opt
                => opt.MapFrom(src => src.Items)).ReverseMap();

        // Booking
        CreateMap<Booking, V1.Booking.Response.BookingResponse>().ReverseMap();
        CreateMap<Booking, V1.Booking.Request.CreateBookingRequest>().ReverseMap();
        CreateMap<PagedResult<Booking>, PagedResult<V1.Booking.Response.BookingResponse>>().ReverseMap();
        CreateMap<Booking, V1.Booking.Response.BookingDetailResponse>().ReverseMap();
        CreateMap<PagedResult<Booking>, PagedResult<V1.Booking.Response.BookingDetailResponse>>()
            .ForMember(dest => dest.Items, opt
                => opt.MapFrom(src => src.Items)).ReverseMap();
    }
}
