using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
   public class SystemNamesQueryDto : QueryObject
    {
        public List<long> SystemNameId { get; set; }
        public List<string> SystemNameDescription { get; set; }

    }
}
