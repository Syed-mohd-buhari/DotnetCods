using CAM.DataTransferObjects.FunctionalityDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.LookUp.AspnetUserRoleModuleMapping
{
    public class AspnetUserRoleModuleMappingCreateDto:AspnetUserRoleModuleMappingDtoGrid
    {
        public List<KeyValuePairDto> RoleResources { get; set; }
        public List<KeyValuePairDto> ModuleResources { get; set; }
    }
}
