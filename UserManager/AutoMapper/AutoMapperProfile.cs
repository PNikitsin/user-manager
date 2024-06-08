using AutoMapper;
using UserManager.Entities;
using UserManager.ViewModels;

namespace UserManager.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ApplicationUser, UserViewModel>();
        }
    }
}