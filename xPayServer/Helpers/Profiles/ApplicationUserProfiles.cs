using AutoMapper;
using xPayServer.Models;
using xPayServer.Models.Dtos;

namespace xPayServer.Helpers.Profiles;

class ApplicationUserProfiles : Profile
{
        public ApplicationUserProfiles()
        {
                CreateMap<ApplicationUser, ApplicationUserForDisplayDto>()
                .ForMember(x => x.Username, opt => opt.MapFrom(source => source.UserName));
        }
}