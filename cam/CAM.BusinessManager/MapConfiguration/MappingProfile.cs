using AutoMapper;
using CAM.DataTransferObjects.Entita.DesignComponent;
using CAM.DataTransferObjects.Entita.LcmEngineering;
using CAM.DataTransferObjects.Entita.MajorHardwareBuild;
using CAM.DataTransferObjects.Entita.MajorSoftwareBuild;
using CAM.DataTransferObjects.Entita.PlannedActivity;
using CAM.DataTransferObjects.Entita.SystemType;
using CAM.DataTransferObjects.LookUp;
using CAM.DataTransferObjects.LookUp.Activity;
using CAM.DataTransferObjects.LookUp.Location;
using CAM.DataTransferObjects.LookUp.ReasonCheckbox;
using CAM.DataTransferObjects.LookUp.SubDomainSpoc;
using CAM.Entities.Models;
using CAM.Entities.Models.Cross;
using CAM.Entities.Models.Lookup;

namespace CAM.BusinessManager.MapConfiguration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ActivityStatus, ActivityStatusDto>().ReverseMap();
            CreateMap<DesignComponent, DesignComponentDto>().ReverseMap();
            CreateMap<LcmEngineering, LcmEngineeringDto>().ReverseMap();
            CreateMap<MajorHardwareBuild, MajorHardwareBuildDto>().ReverseMap();
            CreateMap<MajorSoftwareBuild, MajorSoftwareBuildDto>().ReverseMap();
            CreateMap<PlannedActivity, PlannedActivityDto>().ReverseMap();
            CreateMap<SubDomainSpoc, SubDomainSpocGridDto>()
            .ForMember(x => x.Id, s => s.MapFrom(x => x.SubDomainSpocId))
            .ForMember(x => x.Description, s => s.MapFrom(x => x.SubDomainSpocDescription))
            .ForMember(x => x.isEdu, s => s.MapFrom(x => x.isEdu))
            .ForMember(x => x.isSubDomain, s => s.MapFrom(x => x.isSubDomain))
            .ForMember(x => x.LastModified, s => s.MapFrom(x => x.ModificationDate))
            .ForMember(x => x.LastModifiedBy, s => s.MapFrom(x => x.ModificationUserEntity.UserName))
            .ReverseMap();
            //.ForMember(x=>x.SubDomainSpocId, s=>s.MapFrom(x=>x.Id))
            //.ForMember(x=>x.SubDomainSpocDescription, s=>s.MapFrom(x=>x.Description));

            CreateMap<SystemType, SystemTypeDto>().ReverseMap();
            CreateMap<SystemTypesMajorHardwareBuild, SystemTypesMajorHardwareBuildDto>().ReverseMap();

            CreateMap<Location, LocationDto>()
           .ForMember(x => x.Description, s => s.MapFrom(x => x.LocationDescription))
           .ForMember(x => x.Id, s => s.MapFrom(x => x.LocationId))
           .ReverseMap().ForMember(x => x.LocationDescription, s => s.MapFrom(x => x.Description))
           .ForMember(x => x.LocationId, s => s.MapFrom(x => x.Id));

            CreateMap<ReasonCheckboxResource, ReasonCheckboxDto>().ReverseMap();
            CreateMap<SupportedResource, TipologicaGridDtoRule>().ReverseMap();

        }
    }
}
