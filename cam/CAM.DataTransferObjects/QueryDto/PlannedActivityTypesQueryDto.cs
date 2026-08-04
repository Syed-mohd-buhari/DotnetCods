using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.QueryDto
{
    public class PlannedActivityTypesQueryDto : QueryObject
    {
        public List<int> PlannedActivityTypesId { get; set; }
        public List<string> PlannedActivityTypeDescription { get; set; }
        public List<bool> HwOem { get; set; }
        public List<bool> HwSolution { get; set; }
        public List<bool> HwPlatform { get; set; }
        public List<bool> SwOem { get; set; }
        public List<bool> SwProductname { get; set; }
        public List<bool> SwVersion { get; set; }
        public List<bool> SubNetworkService { get; set; }
        public List<string> CreationUser { get; set; }
        public DateFilter CreationDate { get; set; }
        public List<string> ModificationUser { get; set; }
        public DateFilter ModificationDate { get; set; }
        public List<bool> LinkedDcRule { get; set; }
        public List<bool> ForLcm { get; set; }
        public List<bool> ForAsset { get; set; }
        public List<bool> ForDesignAspect { get; set; }
        public List<string> ForServicePlan { get; set; }

    }
}
