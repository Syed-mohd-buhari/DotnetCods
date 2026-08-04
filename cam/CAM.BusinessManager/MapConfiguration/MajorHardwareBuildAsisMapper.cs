using AutoMapper;
using CAM.DataTransferObjects.LookUp.MajorHardwareBuildAsIs;
using CAM.Entities.Models;
using CAM.Entities.Models.Lookup;

namespace CAM.BusinessManager.MapConfiguration
{
    public class MajorHardwareBuildAsisMapper:Profile
    {
        public MajorHardwareBuildAsisMapper()
        {
            CreateMap<MajorHardwareBuildAsIs, MajorHardwareBuildAsIsDtoGrid>()
            .ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationUserEntity.Email))
            .ForMember(x => x.LastModified, s => s.MapFrom(src => src.ModificationDate))
            .ForMember(x => x.OrgEqpManuFacturerId, s => s.MapFrom(src => src.OriginalEquipmentManufacturerId))
            .ForMember(x => x.OrgEqpManuFacturerDesc, s => s.MapFrom(src => src.OriginalEquipmentManufacturer.OriginalEquipmentManufacturerDescription))
            .ForMember(x => x.PlatformDesc, s => s.MapFrom(src => src.Platform.PlatformDescription))
            .ForMember(x => x.BuildConstructionDesc, s => s.MapFrom(src => src.BuildConstruction.BuildConstructionDescription));
        }
    }
}
