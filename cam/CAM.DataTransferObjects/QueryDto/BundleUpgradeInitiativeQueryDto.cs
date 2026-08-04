using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
   public class BundleUpgradeInitiativeQueryDto : QueryObject
    {
        public List<long> BundleUpgradeInitiativeId { get; set; }
        public List<short> OriginalEquipmentManufacturer { get; set; }
        public List<string> VnfType { get; set; }
        public List<string> VerticalOwner { get; set; }
        public List<string> OemCertifiedRelease { get; set; }
        public List<string> Remarks { get; set; }
        public List<string> Spare1Json { get; set; }
        public List<string> LastModifiedBy { get; set; }

        public DateFilter LastModifiedValue { get; set; }
    }
}
