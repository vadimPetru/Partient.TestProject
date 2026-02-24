using AutoMapper;
using Partient.TestProject.Application.DTO_s;
using Partient.TestProject.Domain.Models;

namespace Partient.TestProject.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PatientRequest, Patient>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}
