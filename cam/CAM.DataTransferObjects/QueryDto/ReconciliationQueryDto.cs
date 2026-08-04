using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class ReconciliationQueryDto: QueryObject
    {
        public List<string> OpCo { get; set; }
        public List<string> Oem { get; set; }
        public List<string> ElementName { get; set; }
        public List<string> DeploymentStatus { get; set; }
        public List<string> CurrentSWVersion { get; set; }
        public List<string> NewSWVersion { get; set; }
        public List<string> Status { get; set; }
        public List<long> AssetsId { get;set; }

    }
}
