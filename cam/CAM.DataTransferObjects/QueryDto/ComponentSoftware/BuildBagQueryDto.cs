using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto.ComponentSoftware
{
    public class BuildBagQueryDto : QueryObject
    {
        public List<long> BuildBagId { get; set; }
        public List<string> BuildBagDescription { get; set; }

        public List<string> BagVersion { get; set; }

        public List<string> AssociatedWithLcm { get; set; }

        public List<long> MappedComponentSoftwareBuild { get; set; }

        public List<short> Opco { get; set; }
        public List<long> Dcf { get; set; }

    }
}