using AutoMapper;
using CAM.DataTransferObjects.LookUp.AssetMapInfo;
using CAM.DataTransferObjects.LookUp.MajorHardwareBuildAsIs;
using CAM.Entities.Models.OMC;

namespace CAM.BusinessManager.MapConfiguration
{
    public class AssetMapInfoMapper:Profile
    {
        public AssetMapInfoMapper()
        {
           CreateMap<AssetMapInfo, AssetMapInfoDtoGrid>()
          .ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationUserEntity.Email))
          .ForMember(x => x.LastModified, s => s.MapFrom(src => src.ModificationDate));
        }
    }
}
