using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class DaMigrationStatusQueryDto : QueryObject
    {
        public List<long> DaMigrationStatusId { get; set; }
        public List<long> PlannedActivityId { get; set; }
        public List<string> Opco { get; set; }
        public List<string> Location { get; set; }

        public List<string> Status { get; set; }
 
    }
}
