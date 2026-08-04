using AutoMapper;
using CAM.DataTransferObjects.LookUp.AspnetUserRoleModuleMapping;
using CAM.DataTransferObjects.LookUp.MajorHardwareBuildAsIs;
using CAM.Entities.Models.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.BusinessManager.MapConfiguration
{
    public class AspnetUserRoleModuleMappingMapper:Profile
    {
        public AspnetUserRoleModuleMappingMapper()
        {
            CreateMap<AspNetUserRolePermissions, AspnetUserRoleModuleMappingDtoGrid>()
            .ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationUserEntity.Email))
            .ForMember(x => x.LastModified, s => s.MapFrom(src => src.ModificationDate))
            .ForMember(x => x.RoleName, s => s.MapFrom(src => src.Role.Name))
            .ForMember(x => x.ModuleName, s => s.MapFrom(src => src.Module.Module));

        }
    }
}
