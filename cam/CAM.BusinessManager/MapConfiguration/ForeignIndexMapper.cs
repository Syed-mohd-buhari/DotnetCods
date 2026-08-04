using AutoMapper;
using CAM.Entities;
using CAM.Entities.Models;
using System.Linq;
using CAM.BusinessManager.ExtensionMethod.SystemType;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.DesignComponent;
using CAM.DataTransferObjects.ForeignIndex;
using CAM.Entities.Models.ForeignIndex;

namespace CAM.BusinessManager.MapConfiguration
{
    public class ForeignIndexMapper : Profile
    {

        private IRepositoryWrapper _repositoryWrapper;

        public ForeignIndexMapper()
        {
            CreateMap<FI_SessionDto, FI_Session>()
                .ReverseMap()
                .ForMember(x => x.LastModified, opt => opt.MapFrom(x => x.ModificationDate))
                .ForMember(x => x.LastModifiedBy, opt => opt.MapFrom(x => x.ModificationUserEntity.Email));

            CreateMap<FI_DesignComponentDto, FI_DesignComponent>()
                .ReverseMap()
                .ForMember(x => x.LastModified, opt => opt.MapFrom(x => x.ModificationDate))
                .ForMember(x => x.LastModifiedBy, opt => opt.MapFrom(x => x.ModificationUserEntity.Email));
            CreateMap<FI_SystemTypesDto, FI_SystemTypes>()
                .ReverseMap()
                .ForMember(x => x.LastModified, opt => opt.MapFrom(x => x.ModificationDate))
                .ForMember(x => x.LastModifiedBy, opt => opt.MapFrom(x => x.ModificationUserEntity.Email));
        }
    }
}