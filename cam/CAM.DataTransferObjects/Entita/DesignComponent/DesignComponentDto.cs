using System;
using System.ComponentModel;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;

namespace CAM.DataTransferObjects.Entita.DesignComponent
{
    public  class DesignComponentDto : GridDtoBase
    {
        //[DisplayName("Service Boundary")]
        //public string ServiceApplication { get; set; }
        [IgnoreGrid]
        public bool? GdprRelevant { get; set; }
        

    }
}