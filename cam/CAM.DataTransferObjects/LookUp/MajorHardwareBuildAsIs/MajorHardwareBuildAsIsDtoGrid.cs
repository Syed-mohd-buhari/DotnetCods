using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CAM.DataTransferObjects.LookUp.MajorHardwareBuildAsIs
{
    public class MajorHardwareBuildAsIsDtoGrid:GridDtoBase
    {
        [OrderGrid(Order = 1)]
        [Default]
        public long MajorHardwareBuildAsisId { get; set; }
        [OrderGrid(Order = 2)]
        [Default]
        public string HardwareType { get; set; }
        [OrderGrid(Order = 3)]
        [Default]
        public string HardwareSolution { get; set; }
        [IgnoreGrid]
        public long OrgEqpManuFacturerId { get; set; }
        [OrderGrid(Order = 4)]
        [Default]
        [DisplayName("OrgEqpManuFacturer")]
        public string OrgEqpManuFacturerDesc { get; set; }
        [IgnoreGrid]
        public long BuildConstructionId { get; set; }
        [OrderGrid(Order = 5)]
        [Default]
        [DisplayName("BuildConstruction")]
        public string BuildConstructionDesc { get; set; }
        [IgnoreGrid]
        public long PlatformId { get; set; }
        [Default]
        [OrderGrid(Order = 6)]
        [DisplayName("Platform")]
        public string PlatformDesc { get; set; }
       
       
    }
    
}
