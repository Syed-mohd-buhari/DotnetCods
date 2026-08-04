using AutoMapper;
using CAM.DataTransferObjects.Entita.OMC.AssetAsisSdiSwitchInfo;
using CAM.Entities.Models.OMC;

namespace CAM.BusinessManager.MapConfiguration.OMC
{
    public class AssetAsisSdiSwitchInfoMapper:Profile
    {
        public AssetAsisSdiSwitchInfoMapper()
        {
            CreateMap<AssetAsIsSdiSwitchInfo, AssetAsIsSdiSwitchInfoGridDto>()
         .ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationUserEntity.Email))
         .ForMember(x => x.LastModified, s => s.MapFrom(src => src.ModificationDate));
        }
    }
}
