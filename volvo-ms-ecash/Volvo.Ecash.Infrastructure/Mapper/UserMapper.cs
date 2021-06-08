using AutoMapper;
using Volvo.Ecash.Domain.Entities;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Infrastructure.Mapper
{
    public class UserMapper : Profile
    {

        public UserMapper()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<UserDto, UserLogin>();
            CreateMap<UserLogin, UserDto>();

            CreateMap<User, UserLogin>();
            CreateMap<UserLogin, User>();

            CreateMap<RefreshToken, RefreshTokenDto>();
            CreateMap<RefreshTokenDto, RefreshToken>();


            //Trocar de local
            CreateMap<DocumentUpload, DocumentUploadDto>();
            CreateMap<DocumentUploadDto, DocumentUpload>();
        }

    }
}
