using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class ArchivedLcmEngineeringQueryDto : QueryObject
    {
        public List<long> LcmEngineeringId { get; set; }
        public List<long> DesignComponent { get; set; }
        public List<short> OpCo { get; set; }
        public List<long> DesignComponentId { get; set; }
        public List<long> DesignComponentFamily { get; set; }
        public List<short> LcmDeploymentStatus { get; set; }
        public List<long> VodafoneName { get; set; }
        public List<long> SubnetworkBoundary { get; set; }
        public DateFilter StartDate { get; set; }
        public DateFilter EndDate { get; set; }
        public List<bool> Archived { get; set; }
        public new DateFilter LastModifiedValue { get; set; }

        public List<int> VerticalId { get; set; }
        public List<string> VerticalName { get; set; }
        public List<string> ResourceKey { get; set; }
        public List<long> BuildBagDescription { get; set; }
    }
}
