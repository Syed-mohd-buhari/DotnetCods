using AutoMapper;
using CAM.DataTransferObjects.Entita.OMC.AssetAsIsSdiInfo;
using CAM.Entities.Models.OMC;

namespace CAM.BusinessManager.MapConfiguration.OMC
{
    public class AssetAsisSdiInfoMapper:Profile
    {
        public AssetAsisSdiInfoMapper()
        {
            CreateMap<AssetAsIsSdiInfo, AssetAsIsSdiInfoDtoGrid>()
                .ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationUserEntity.Email))
                .ForMember(x => x.LastModified, s => s.MapFrom(src => src.ModificationDate));
        }

    }
}
