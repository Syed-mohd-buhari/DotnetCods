using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using DocumentFormat.OpenXml.Drawing.Charts;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class MajorSwBuildsDesignContactQueryDto : QueryObject
    {
        
        public List<long> MajorSwBuidlsDesignContactId { get; set; }
     
        public List<long> DesignContactId { get; set; }
        
        public List<string> DesignContact { get; set; }

        public List<long> MajorSoftwareBuildId { get; set; }

        public List<string> MajorSoftwareBuild { get; set; }
    }
}