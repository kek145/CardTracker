using AutoMapper;
using System.Text;
using CardTracker.Domain.Models;
using CardTracker.Domain.Requests.Registration;

namespace CardTracker.Domain.Mappings;

public class RequestToDomain : Profile
{
    public RequestToDomain()
    {
        CreateMap<RegistrationRequest, User>()
            .ForMember(dest => dest.FirstName,
                opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName,
                opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Email,
                opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PasswordHash,
                opt => opt.MapFrom(src => Encoding.UTF8.GetBytes(src.Password)))
            .ForMember(dest => dest.PasswordSalt,
                opt => opt.MapFrom(src => Encoding.UTF8.GetBytes(src.Password)));
    }
}