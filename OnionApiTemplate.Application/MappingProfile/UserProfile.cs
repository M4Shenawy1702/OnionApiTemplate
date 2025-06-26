using AutoMapper;
using OnionApiTemplate.Application.DOTs.Auth;
using OnionApiTemplate.Domain.Entities;

namespace OnionApiTemplate.Application.MappingProfile
{
    internal class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterRequest, ApplicationUser>();
        }
    }
}
