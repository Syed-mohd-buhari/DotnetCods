using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class PlannedActivityAssociationQueryDto : QueryObject
    {
        public List<long> LcmengineeringId { get; set; }
    }
}