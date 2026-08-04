using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.LookUp.AspnetUserRoleModuleMapping
{
    public class AspnetUserRoleModuleMappingUpdateDto: GridDtoBase
    {
        public List<DropdownAspnetModuleList> ModuleResources { get; set; }
        public int RoleId { get; set; }
        public string ModuleId { get; set; }
        public string RoleName { get; set; }
        public short PermissionLevel { get; set; }
        public List<UserRoleModuleMappingDto> RoleModuleResources { get; set; }
        public string Description { get; set; }
        public List<AbstractionRoleOrder> AbstractionRoleOrders { get; set; }
        
    }
    public class UserRoleModuleMappingDto
    {
        public int RoleId { get; set; }
        public string RoleDescription { get; set; }
        public string ModuleId { get; set; }
        public short PermissionLevel { get; set; }
        public string Description { get; set; }
        public List<AbstractionRoleOrder> AbstractionRoleOrders { get; set; }
    }
    public class DropdownAspnetModuleList
    {
        public int Key { get; set; }
        public string Value { get; set; }
        public string ModulePath { get; set; }
        public string Category { get; set; }
        public string Menu { get; set; }
    }

    public class ModelPermissionDetials
    {
        public List<int> ModuleId { get; set; } = new();
        public Dictionary<int, short?> PermissionIds { get; set; } = new();
    }
    public class AbstractionRoleOrder
    {
        public string Name { get; set; }
        public int Order { get;set; }
        public bool IsVisible { get; set; } = false;
    }
}
