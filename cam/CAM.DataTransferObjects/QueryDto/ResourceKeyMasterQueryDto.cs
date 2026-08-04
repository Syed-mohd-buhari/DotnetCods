using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.QueryDto
{
    public class ResourceKeyMasterQueryDto : QueryObject
    {
        public List<long> ResourceKeyMasterId { get; set; }
        public List<long> ResourceTypesId { get; set; }
        public List<string> ResourceKey { get; set; }
        public List<long> DcfId { get; set; }
        public List<short> OpCoId { get; set; }
        public List<long> EventId { get; set; }
        public List<long> DcId { get; set; }
        public List<string> CreationUser { get; set; }
        public DateFilter CreationDate { get; set; }
        public List<string> ModificationUser { get; set; }
        public DateFilter ModificationDate { get; set; }
        public List<string> KeyStatus { get; set; }
        public List<long> DesignComponentFamilyName { get; set; }
        public List<short> OpCo { get; set; }
        public List<long> ResourceType { get; set; }
        public List<int> LifeCycleId { get; set; }

        public List<string> ElementName { get; set; }
        public List<long> BagName { get; set; }
    }
}
