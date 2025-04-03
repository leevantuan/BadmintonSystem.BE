using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.ChatRoom;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Enumerations;
using static BadmintonSystem.Contract.Services.V1.Category.Response;
using DayOfWeek = BadmintonSystem.Domain.Entities.DayOfWeek;
using Request = BadmintonSystem.Contract.Services.V1.Category.Request;
using V1 = BadmintonSystem.Contract.Services.V1;

namespace BadmintonSystem.Application.Mappers;

public class ServiceV1Profile : Profile
{
    public ServiceV1Profile()
    {
        // Tenant
        CreateMap<Tenant, V1.Tenant.Response.TenantResponse>().ReverseMap();
        CreateMap<Tenant, V1.Tenant.Request.CreateTenantRequest>().ReverseMap();

        // ChatRoom
        CreateMap<ChatRoom, Response.ChatRoomResponse>().ReverseMap();
        CreateMap<ChatRoom, Response.GetChatRoomByIdResponse>().ReverseMap();

        // ChatMessage
        CreateMap<ChatMessage, V1.ChatMessage.Response.ChatMessageResponse>().ReverseMap();

        // CLUB INFORMATION
        CreateMap<ClubInformation, V1.ClubInformation.Request.CreateClubInformationRequest>().ReverseMap();
        CreateMap<ClubImage, V1.ClubImage.Request.CreateClubImageRequest>().ReverseMap();

        // App User
        CreateMap<AppUser, V1.User.Response.AppUserResponse>().ReverseMap();
        CreateMap<AppUser, V1.User.Response.GetUserInfoResponse>().ReverseMap();

        // Address
        CreateMap<Address, V1.Address.Response.AddressResponse>().ReverseMap();
        CreateMap<Address, V1.Address.Request.CreateAddressRequest>().ReverseMap();

        // Payment method
        CreateMap<PaymentMethod, V1.User.Response.PaymentMethodByUserResponse>()
            .ForMember(dest => (DefaultEnum)dest.IsDefault, opt
                => opt.MapFrom(src => src.IsDefault));
        CreateMap<PaymentMethod, V1.PaymentMethod.Request.CreatePaymentMethodRequest>().ReverseMap();
        CreateMap<PaymentMethod, V1.User.Response.NotificationByUserResponse>().ReverseMap();

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

        // Price
        CreateMap<Price, V1.Price.Response.PriceResponse>().ReverseMap();
        CreateMap<Price, V1.Price.Request.CreatePriceRequest>().ReverseMap();
        CreateMap<PagedResult<Price>, PagedResult<V1.Price.Response.PriceResponse>>().ReverseMap();
        CreateMap<Price, V1.Price.Response.PriceDetailResponse>().ReverseMap();
        CreateMap<PagedResult<Price>, PagedResult<V1.Price.Response.PriceDetailResponse>>()
            .ForMember(dest => dest.Items, opt
                => opt.MapFrom(src => src.Items)).ReverseMap();

        // Yard Price
        CreateMap<YardPrice, V1.YardPrice.Response.YardPriceResponse>().ReverseMap();
        CreateMap<YardPrice, V1.YardPrice.Request.CreateYardPriceRequest>().ReverseMap();
        CreateMap<PagedResult<YardPrice>, PagedResult<V1.YardPrice.Response.YardPriceResponse>>().ReverseMap();
        CreateMap<YardPrice, V1.YardPrice.Response.YardPriceDetailResponse>().ReverseMap();
        CreateMap<PagedResult<YardPrice>, PagedResult<V1.YardPrice.Response.YardPriceDetailResponse>>()
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
        CreateMap<Review, V1.Review.Response.GetReviewDetailResponse>().ReverseMap();
        CreateMap<Review, V1.Review.Response.ReviewResponse>().ReverseMap();
        CreateMap<Review, V1.Review.Request.CreateReviewRequest>().ReverseMap();
        CreateMap<PagedResult<Review>, PagedResult<V1.Review.Response.ReviewResponse>>().ReverseMap();
        CreateMap<Review, V1.Review.Response.ReviewDetailResponse>().ReverseMap();
        CreateMap<PagedResult<Review>, PagedResult<V1.Review.Response.ReviewDetailResponse>>()
            .ForMember(dest => dest.Items, opt
                => opt.MapFrom(src => src.Items)).ReverseMap();

        // Review Image
        CreateMap<ReviewImage, V1.ReviewImage.Request.CreateReviewImageRequest>().ReverseMap();
        CreateMap<ReviewImage, V1.ReviewImage.Response.ReviewImageDetailResponse>().ReverseMap();

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
        CreateMap<Booking, V1.Booking.Request.CreateBooking>().ReverseMap();
        CreateMap<Booking, V1.Booking.Response.BookingResponse>().ReverseMap();
        CreateMap<Booking, V1.Booking.Request.CreateBookingRequest>().ReverseMap();
        CreateMap<PagedResult<Booking>, PagedResult<V1.Booking.Response.BookingResponse>>().ReverseMap();
        CreateMap<Booking, V1.Booking.Response.BookingDetail>().ReverseMap();
        CreateMap<PagedResult<Booking>, PagedResult<V1.Booking.Response.BookingDetail>>()
            .ForMember(dest => dest.Items, opt
                => opt.MapFrom(src => src.Items)).ReverseMap();

        // Day Off
        CreateMap<DayOff, V1.DayOff.Response.DayOffResponse>().ReverseMap();
        CreateMap<DayOff, V1.DayOff.Request.CreateDayOffRequest>().ReverseMap();
        CreateMap<PagedResult<DayOff>, PagedResult<V1.DayOff.Response.DayOffResponse>>().ReverseMap();
        CreateMap<DayOff, V1.DayOff.Response.DayOffDetailResponse>().ReverseMap();
        CreateMap<PagedResult<DayOff>, PagedResult<V1.DayOff.Response.DayOffDetailResponse>>()
            .ForMember(dest => dest.Items, opt
                => opt.MapFrom(src => src.Items)).ReverseMap();

        // Day of week
        CreateMap<DayOfWeek, V1.DayOfWeek.Response.DayOfWeekResponse>().ReverseMap();
        CreateMap<DayOfWeek, V1.DayOfWeek.Request.CreateDayOfWeekRequest>().ReverseMap();
        CreateMap<PagedResult<DayOfWeek>, PagedResult<V1.DayOfWeek.Response.DayOfWeekResponse>>().ReverseMap();
        CreateMap<DayOfWeek, V1.DayOfWeek.Response.DayOfWeekDetailResponse>().ReverseMap();
        CreateMap<PagedResult<DayOfWeek>, PagedResult<V1.DayOfWeek.Response.DayOfWeekDetailResponse>>()
            .ForMember(dest => dest.Items, opt
                => opt.MapFrom(src => src.Items)).ReverseMap();

        // Provider
        CreateMap<Provider, V1.Provider.Response.ProviderResponse>().ReverseMap();
        CreateMap<Provider, V1.Provider.Request.CreateProviderRequest>().ReverseMap();
        CreateMap<PagedResult<Provider>, PagedResult<V1.Provider.Response.ProviderResponse>>().ReverseMap();
        CreateMap<Provider, V1.Provider.Response.ProviderDetailResponse>().ReverseMap();
        CreateMap<PagedResult<Provider>, PagedResult<V1.Provider.Response.ProviderDetailResponse>>()
            .ForMember(dest => dest.Items, opt
                => opt.MapFrom(src => src.Items)).ReverseMap();

        // Bill
        CreateMap<Bill, V1.Bill.Response.BillResponse>().ReverseMap();
        CreateMap<Bill, V1.Bill.Request.CreateBillRequest>().ReverseMap();
        CreateMap<PagedResult<Bill>, PagedResult<V1.Bill.Response.BillResponse>>().ReverseMap();
        CreateMap<Bill, V1.Bill.Response.BillDetailResponse>().ReverseMap();
        CreateMap<PagedResult<Bill>, PagedResult<V1.Bill.Response.BillDetailResponse>>()
            .ForMember(dest => dest.Items, opt
                => opt.MapFrom(src => src.Items)).ReverseMap();

        // Inventory Receipt
        CreateMap<InventoryReceipt, V1.InventoryReceipt.Response.InventoryReceiptResponse>().ReverseMap();
        CreateMap<InventoryReceipt, V1.InventoryReceipt.Request.CreateInventoryReceiptRequest>().ReverseMap();
        CreateMap<PagedResult<InventoryReceipt>, PagedResult<V1.InventoryReceipt.Response.InventoryReceiptResponse>>()
            .ReverseMap();
        CreateMap<InventoryReceipt, V1.InventoryReceipt.Response.InventoryReceiptDetailResponse>().ReverseMap();
        CreateMap<PagedResult<InventoryReceipt>,
                PagedResult<V1.InventoryReceipt.Response.InventoryReceiptDetailResponse>>()
            .ForMember(dest => dest.Items, opt
                => opt.MapFrom(src => src.Items)).ReverseMap();

        // Time Slot Of Week
        CreateMap<TimeSlotOfWeek, V1.TimeSlotOfWeek.Response.TimeSlotOfWeekResponse>().ReverseMap();
        CreateMap<TimeSlotOfWeek, V1.TimeSlotOfWeek.Request.CreateTimeSlotOfWeekRequest>().ReverseMap();
        CreateMap<PagedResult<TimeSlotOfWeek>, PagedResult<V1.TimeSlotOfWeek.Response.TimeSlotOfWeekResponse>>()
            .ReverseMap();
        CreateMap<TimeSlotOfWeek, V1.TimeSlotOfWeek.Response.TimeSlotOfWeekDetailResponse>().ReverseMap();
        CreateMap<PagedResult<TimeSlotOfWeek>, PagedResult<V1.TimeSlotOfWeek.Response.TimeSlotOfWeekDetailResponse>>()
            .ForMember(dest => dest.Items, opt
                => opt.MapFrom(src => src.Items)).ReverseMap();

        // Fixed Schedule
        CreateMap<FixedSchedule, V1.FixedSchedule.Response.FixedScheduleResponse>().ReverseMap();
        CreateMap<FixedSchedule, V1.FixedSchedule.Request.CreateFixedScheduleRequest>().ReverseMap();
        CreateMap<PagedResult<FixedSchedule>, PagedResult<V1.FixedSchedule.Response.FixedScheduleResponse>>()
            .ReverseMap();
        CreateMap<FixedSchedule, V1.FixedSchedule.Response.FixedScheduleDetailResponse>().ReverseMap();
        CreateMap<PagedResult<FixedSchedule>, PagedResult<V1.FixedSchedule.Response.FixedScheduleDetailResponse>>()
            .ForMember(dest => dest.Items, opt
                => opt.MapFrom(src => src.Items)).ReverseMap();
    }
}
