using AutoMapper;
using UsersApi.Models;

namespace UsersApi.Dtos.Profiles
{
    public class UserProfile : Profile
    {

        public UserProfile()
        {
            CreateMap<CreateUserDto, User>();
        }

    }
}
