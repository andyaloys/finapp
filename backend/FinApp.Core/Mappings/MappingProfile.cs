using AutoMapper;
using FinApp.Core.DTOs.Stpb;
using FinApp.Domain.Entities;

namespace FinApp.Core.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // STPB mappings
        CreateMap<Stpb, StpbDto>()
            .ForMember(dest => dest.CreatorName, 
                opt => opt.MapFrom(src => src.Creator != null ? src.Creator.FullName : string.Empty));
        
        CreateMap<CreateStpbDto, Stpb>();
        
        CreateMap<UpdateStpbDto, Stpb>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Creator, opt => opt.Ignore());
    }
}
