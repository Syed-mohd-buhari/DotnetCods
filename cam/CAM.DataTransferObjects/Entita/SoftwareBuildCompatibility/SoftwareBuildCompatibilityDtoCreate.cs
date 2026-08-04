using CAM.DataAttributes.Grid;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Presentation;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CAM.DataTransferObjects.Entita.SoftwareBuildCompatibility
{
    public class SoftwareBuildCompatibilityDtoCreate
    {
        public long SoftwareBuildCompatibilityId { get; set; }
        public long MajorSoftwareBuildId { get; set; }

        public long BundleMajorSoftwareBuildId { get; set; }

        public short BundleType { get; set; }
         
    }

    public class SoftwareBuildCompatibilityListDtoCreateUpdate
    {
        public long MajorSoftwareBuildId { get; set; }
        public List<long> TCPSoftwareCompatibilityId { get; set; }
        public List<long> TCISoftwareCompatibilityId { get; set; }

    }
    public class SoftwareBuildCompatibilityListDtoCreate
    {
        public long MajorSoftwareBuildId { get; set; }
        public List<SoftwareBuildCompatibilityDtoCreate> SoftwareBuildCompatibilityList { get; set; }
        
    }
}