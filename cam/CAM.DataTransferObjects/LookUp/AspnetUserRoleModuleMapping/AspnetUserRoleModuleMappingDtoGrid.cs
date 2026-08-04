using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using CAM.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.LookUp.AspnetUserRoleModuleMapping
{
    public class AspnetUserRoleModuleMappingDtoGrid:GridDtoBase
    {
        [OrderGrid(Order = 1)]
        [Default]
        [DisplayName("Id")]
        public int AspNetUserRolePermissionId { get; set; }
       
        [IgnoreGrid]
        public int? RoleId { get; set; }

        [OrderGrid(Order = 2)]
        [Default]
        [DisplayName("Role Name")]
        public string RoleName {  get; set; }

        [IgnoreGrid]
        public int? ModuleId { get; set; }

        [OrderGrid(Order = 3)]
        [Default]
        [DisplayName("Module Name")]
        public string ModuleName { get; set; }

        [OrderGrid(Order = 4)]
        [Default]
        [DisplayName("Permission Level")]
        public short? PermissionLevel { get; set; }

    }
}
