using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.SystemType
{
 public class SystemTypeReleatedMajorEntity
    {
        public List<long> IdMajorSoftwareSameAssetCategory { get; set; }
        public List<long> IdMajorSoftwareOtherAssetCategory { get; set; }
        public List<long> IdMajorHardwareSameAssetCategory { get; set; }
        public List<long> IdMajorHardwareOtherAssetCategory { get; set; }
    }
}
