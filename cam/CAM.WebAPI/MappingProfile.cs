using AutoMapper;
using CAM.DataTransferObjects.Entita.ComponentSoftware;
using CAM.DataTransferObjects.Entita.DesignComponent;
using CAM.DataTransferObjects.Entita.LcmEngineering;
using CAM.DataTransferObjects.Entita.MajorSoftwareBuild;
using CAM.DataTransferObjects.Entita.PlannedActivity;
using CAM.DataTransferObjects.Entita.SystemType;
using CAM.DataTransferObjects.LookUp.Activity;
using CAM.DataTransferObjects.LookUp.Asset;
using CAM.Entities.Models;
using CAM.Entities.Models.Cross;
using CAM.Entities.Models.Lookup;

namespace CAM.WebAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ActivityStatus, ActivityStatusDto>().ReverseMap();
            CreateMap<AssetClass, AssetClassDto>().ReverseMap();
            CreateMap<AssetType, AssetTypeDto>().ReverseMap();
            CreateMap<DesignComponent, DesignComponentDto>().ReverseMap();
            CreateMap<LcmEngineering, LcmEngineeringDto>().ReverseMap();
            CreateMap<MajorSoftwareBuild, MajorSoftwareBuildDto>().ReverseMap();
           CreateMap<PlannedActivity, PlannedActivityDto>().ReverseMap();
            CreateMap<SystemType, SystemTypeDto>().ReverseMap();
            CreateMap<SystemTypesMajorHardwareBuild, SystemTypesMajorHardwareBuildDto>().ReverseMap();
         

        }
    }

 

}
