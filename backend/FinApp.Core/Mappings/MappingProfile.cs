using AutoMapper;
using FinApp.Core.DTOs.Stpb;
using FinApp.Core.DTOs.User;
using FinApp.Core.DTOs.Program;
using FinApp.Core.DTOs.Kegiatan;
using FinApp.Core.DTOs.Output;
using FinApp.Core.DTOs.Suboutput;
using FinApp.Core.DTOs.Komponen;
using FinApp.Core.DTOs.Subkomponen;
using FinApp.Core.DTOs.Akun;
using FinApp.Core.DTOs.Item;
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
        
        CreateMap<CreateStpbDto, Stpb>()
            .ForMember(dest => dest.KodeProgram, opt => opt.MapFrom(src => src.ProgramId))
            .ForMember(dest => dest.KodeKegiatan, opt => opt.MapFrom(src => src.KegiatanId))
            .ForMember(dest => dest.KodeOutput, opt => opt.MapFrom(src => src.OutputId))
            .ForMember(dest => dest.KodeSuboutput, opt => opt.MapFrom(src => src.SuboutputId))
            .ForMember(dest => dest.KodeKomponen, opt => opt.MapFrom(src => src.KomponenId))
            .ForMember(dest => dest.KodeSubkomponen, opt => opt.MapFrom(src => src.SubkomponenId))
            .ForMember(dest => dest.KodeAkun, opt => opt.MapFrom(src => src.AkunId))
            .ForMember(dest => dest.ItemId, opt => opt.Ignore())
            .ForMember(dest => dest.PPn, opt => opt.MapFrom(src => src.Ppn))
            .ForMember(dest => dest.PPh21, opt => opt.MapFrom(src => src.Pph21))
            .ForMember(dest => dest.PPh22, opt => opt.MapFrom(src => src.Pph22))
            .ForMember(dest => dest.PPh23, opt => opt.MapFrom(src => src.Pph23))
            .ForMember(dest => dest.NilaiBersih, opt => opt.MapFrom(src => src.NilaiTotal));
        
        CreateMap<UpdateStpbDto, Stpb>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Creator, opt => opt.Ignore());

        // User mappings
        CreateMap<User, UserDto>();
        CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

        // Program mappings
        CreateMap<Program, ProgramDto>();
        CreateMap<CreateProgramDto, Program>();
        CreateMap<UpdateProgramDto, Program>();

        // Kegiatan mappings
        CreateMap<Kegiatan, KegiatanDto>();
        CreateMap<CreateKegiatanDto, Kegiatan>();
        CreateMap<UpdateKegiatanDto, Kegiatan>();

        // Output mappings
        CreateMap<Output, OutputDto>();
        CreateMap<CreateOutputDto, Output>();
        CreateMap<UpdateOutputDto, Output>();

        // Suboutput mappings
        CreateMap<Suboutput, SuboutputDto>();
        CreateMap<CreateSuboutputDto, Suboutput>();
        CreateMap<UpdateSuboutputDto, Suboutput>();

        // Komponen mappings
        CreateMap<Komponen, KomponenDto>();
        CreateMap<CreateKomponenDto, Komponen>();
        CreateMap<UpdateKomponenDto, Komponen>();

        // Subkomponen mappings
        CreateMap<Subkomponen, SubkomponenDto>();
        CreateMap<CreateSubkomponenDto, Subkomponen>();
        CreateMap<UpdateSubkomponenDto, Subkomponen>();

        // Akun mappings
        CreateMap<Akun, AkunDto>();
        CreateMap<CreateAkunDto, Akun>();
        CreateMap<UpdateAkunDto, Akun>();

        // Item mappings
        CreateMap<Item, ItemDto>()
            .ForMember(dest => dest.kodeAkun, opt => opt.MapFrom(src => src.Akun != null ? src.Akun.Kode : string.Empty));
        CreateMap<CreateItemDto, Item>();
        CreateMap<UpdateItemDto, Item>();
    }
}
