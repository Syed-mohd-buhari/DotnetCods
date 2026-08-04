using AutoMapper;
using CAM.DataTransferObjects.Entita.OMC.AssetAsIsHwAncillaryData;
using CAM.Entities.Models.OMC;

namespace CAM.BusinessManager.MapConfiguration.OMC
{
    public class AssetAsIsHwAncillaryDataMapper:Profile
    {
        public AssetAsIsHwAncillaryDataMapper()
        {
            CreateMap<AssetAsIsHwAncillaryData, AssetAsIsHwAncillaryDataGridDto>()
         .ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationUserEntity.Email))
         .ForMember(x => x.LastModified, s => s.MapFrom(src => src.ModificationDate));
        }
    }
}
