using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Domain.Entities;

namespace BadmintonSystem.Application.Mapper;
public class ServiceProfile : Profile
{
    public ServiceProfile()
    {
        // V1
        CreateMap<Gender, Contract.Services.V1.Gender.Response.GenderResponse>().ReverseMap();
        CreateMap<Gender, Contract.Services.V1.Gender.Request.GenderRequest>().ReverseMap();
        CreateMap<PagedResult<Gender>, PagedResult<Contract.Services.V1.Gender.Response.GenderResponse>>().ReverseMap();

        // V2
        CreateMap<Gender, Contract.Services.V2.Gender.Response.GenderResponse>().ReverseMap();
        CreateMap<Gender, Contract.Services.V2.Gender.Request.GenderRequest>().ReverseMap();
        CreateMap<PagedResult<Gender>, PagedResult<Contract.Services.V2.Gender.Response.GenderResponse>>().ReverseMap();

        #region ==================== Category =======================

        CreateMap<Category, Contract.Services.V2.Category.Response.CategoryResponse>().ReverseMap();
        CreateMap<Category, Contract.Services.V2.Category.Request.CategoryRequest>().ReverseMap();
        CreateMap<PagedResult<Category>, PagedResult<Contract.Services.V2.Category.Response.CategoryResponse>>().ReverseMap();

        #endregion ==================== Category =======================
    }
}
