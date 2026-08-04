using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class ConstrainInfoQueryDto
    {
        public int? MajorSoftwareBuild { get; set; }
        public List<long?> MajorHardwareBuilds { get; set; }
    }
}
