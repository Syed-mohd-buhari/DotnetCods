using AutoMapper;
using CAM.DataTransferObjects.Entita.VolteKPI;
using CAM.Entities.Models;

namespace CAM.BusinessManager.MapConfiguration
{
    public class VolteKPIMapper : Profile
    {
        public VolteKPIMapper()
        {
            CreateMap<VolteKPIDtoCreate, VolteKPI>();
            CreateMap<VolteKPI, VolteKPIDtoCreate>();
            CreateMap<VolteKPIDtoUpdate, VolteKPI>();
            CreateMap<VolteKPI, VolteKPIDtoUpdate>()
                .ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationUserEntity.Email))
                .ForMember(x => x.LastModified, s => s.MapFrom(src => src.ModificationDate));
            CreateMap<VolteKPI, VolteKPIDtoGrid>()
                .ForMember(x => x.Deleted, s => s.MapFrom(src => src.Deleted))
                .ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationUserEntity.Email))
                .ForMember(x => x.LastModified, s => s.MapFrom(src => src.ModificationDate))
            ;

        }
    }
}
