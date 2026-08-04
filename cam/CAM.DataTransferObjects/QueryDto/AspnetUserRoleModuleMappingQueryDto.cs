using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.QueryDto
{
    public class AspnetUserRoleModuleMappingQueryDto:QueryObject
    {
        public List<long> AspNetUserRolePermissionId { get; set; }
        public List<string> RoleName { get; set; }
        public List<string> ModuleName { get; set; }
        public List<short> PermissionLevel { get; set; }
    }
}
