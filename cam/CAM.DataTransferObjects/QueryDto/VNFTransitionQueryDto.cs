using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
  public  class VnfTransitionQueryDto : QueryObject
    {
        public List<long> VnfTransitionId { get; set; }
        public List<short> VnfDesignComponent { get; set; }
        public List<short> OpCo { get; set; }
        public List<string> VnfType { get; set; }
        public List<string> CurrentRelease { get; set; }
        public List<string> PlannedRelease { get; set; }
        public List<string> ElementName { get; set; }
        public List<string> Location { get; set; }
        public List<short> NfviBundleID { get; set; }
        public List<string> Spare1Json { get; set; }
       
        public List<string> NfviSiteDesignation { get; set; }
        public List<short> EquipmentStatus { get; set; }
        public List<string> LastModifiedBy { get; set; }

        public DateFilter LastModifiedValue { get; set; }
    }
}
