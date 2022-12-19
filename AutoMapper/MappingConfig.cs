using AutoMapper;
using VillaApp_WebAPI.Models;
using VillaApp_WebAPI.Models.Dto;

namespace VillaApp_WebAPI.AutoMapper
{
    public class MappingConfig : Profile
    {
        public MappingConfig() 
        {
            CreateMap<Villa, VillaDTO>();
            CreateMap<VillaDTO, Villa>();

            CreateMap<Villa, VillaCreateDTO>().ReverseMap();
            CreateMap<Villa, VillaUpdateDTO>().ReverseMap();

        }
    }
}
