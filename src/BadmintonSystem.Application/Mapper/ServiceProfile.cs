using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.Gender;
using BadmintonSystem.Domain.Entities;

namespace BadmintonSystem.Application.Mapper;
public class ServiceProfile : Profile
{
    public ServiceProfile()
    {
        CreateMap<Gender, Response.GenderResponse>().ReverseMap();
        CreateMap<Gender, Request.GenderRequest>().ReverseMap();
        CreateMap<PagedResult<Gender>, PagedResult<Response.GenderResponse>>().ReverseMap();
    }
}
