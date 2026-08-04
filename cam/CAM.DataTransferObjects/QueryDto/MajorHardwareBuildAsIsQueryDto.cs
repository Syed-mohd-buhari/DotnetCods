using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class MajorHardwareBuildAsIsQueryDto: QueryObject
    {
        public List<long> MajorHardwareBuildAsisId { get; set; }
        public List<string> OrgEqpManuFacturerDesc { get; set; }
        public List<string> HardwareSolution { get; set; }
        //public List<string>? HardwareSolutionResourceDesc { get; set; }
        public List<string> PlatformDesc { get; set; }
        public List<string> BuildConstructionDesc { get; set; }
        public List<string> HardwareType { get; set; }
    }
}
