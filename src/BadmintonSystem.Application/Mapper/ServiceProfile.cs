using AutoMapper;
using BadmintonSystem.Contract.Services.Gender;
using BadmintonSystem.Domain.Entities;

namespace BadmintonSystem.Application.Mapper;
public class ServiceProfile : Profile
{
    public ServiceProfile()
    {
        CreateMap<Gender, Response.GenderResponse>().ReverseMap();
        CreateMap<Gender, Request>().ReverseMap();
    }
}
