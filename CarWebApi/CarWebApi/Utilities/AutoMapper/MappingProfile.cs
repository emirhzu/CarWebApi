using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace CarWebApi.Utilities.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CarDtoForUpdate, Car>().ReverseMap();
            CreateMap<Car, CarDto>();
            CreateMap<CarDtoForInsertion, Car>();
            CreateMap<UserForRegistrationDto, User>();
        }
    }
}
